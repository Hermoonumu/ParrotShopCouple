using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using Microsoft.IdentityModel.Tokens;
using Npgsql;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Application.Exceptions;
using ParrotShopBackend.Application.Mappers;
using ParrotShopBackend.Domain;
using ParrotShopBackend.Infrastructure.Repos;
using ParrotShopBackend.Application.Extensions;

namespace ParrotShopBackend.Application.Services;



public class AuthService(IUserService _userSvc,
                            IUserRepository _userRepo,
                            IConfiguration _conf,
                            IRefreshTokenRepository _refreshRepo,
                            //IDistributedCache _c,
                            RedisCacheExtension _redis,
                            ICartService _cartSvc) : IAuthService
{
    public async Task<Dictionary<string, string>> RegisterAsync(RegFormDTO rfDTO, bool admin = false)
    {
        User user = UserMapper.FromRegFormDTO(rfDTO);
        if (admin) user.Role = Role.Admin;
        PasswordHasher<User> passwordHasher = new PasswordHasher<User>();
        user.PasswordHash = passwordHasher.HashPassword(user, rfDTO.Password!);
        try
        {
            await _userRepo.AddUserToDBAsync(user);
        }
        catch (DbUpdateException e) when (e.InnerException is PostgresException pgr)
        {
            if (pgr.SqlState == "23505")
            {
                if (pgr.ConstraintName == "AK_Users_Username")
                {
                    throw new UserAlreadyExistsException("Username already exists.");
                }
                else if (pgr.ConstraintName == "IX_Users_Email")
                {
                    throw new UserAlreadyExistsException("Email already exists.");
                }
                else
                {
                    throw;
                }
            }
        }
        user.Cart = await _cartSvc.NewCartAsync();
        user.Cart.UserId = user.Id;
        return await GenerateTokensAsync(user);

    }

    public ClaimsIdentity CreateClaims(User user)
    {
        ClaimsIdentity claims = new();
        claims.AddClaim(new Claim(ClaimTypes.Name, user.Username!));
        claims.AddClaim(new Claim(ClaimTypes.Role, user.Role.ToString()!));
        claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()!));
        claims.AddClaim(new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()));
        return claims;
    }

    public async Task<Dictionary<string, string>> GenerateTokensAsync(User user, bool expired = false)
    {
        Dictionary<string, string> tokens = new Dictionary<string, string>();
        JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
        byte[] key = Encoding.ASCII.GetBytes(_conf["SecSettings:SecretKey"]!);
        SigningCredentials credentials = new SigningCredentials
            (
                new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha512Signature
            );
        SecurityTokenDescriptor tokenDescriptor = new SecurityTokenDescriptor
        {
            Subject = CreateClaims(user),
            Issuer = _conf["API:Issuer"],
            Audience = _conf["API:Audience"],
            NotBefore = DateTime.UtcNow,
            IssuedAt = DateTime.UtcNow,
            Expires = DateTime.UtcNow.Add(expired ? TimeSpan.FromSeconds(0) : TimeSpan.FromMinutes(Int32.Parse(_conf["SecSettings:TokenDurationMinutes"]!))),
            SigningCredentials = credentials
        };
        tokens.Add("AccessToken", handler.WriteToken(handler.CreateToken(tokenDescriptor)));
        byte[] randStringToken = new byte[128];
        RandomNumberGenerator.Create().GetBytes(randStringToken);
        var plainRefreshToken = Convert.ToBase64String(randStringToken)!;
        tokens.Add("RefreshToken", plainRefreshToken);
        var hasher = SHA512.Create();
        await _refreshRepo.AddTokenAsync(new RefreshToken()
        {
            UserID = (long)user.Id!,
            Token = Convert.ToBase64String(hasher.ComputeHash(Encoding.ASCII.GetBytes(plainRefreshToken))!),
            IssuedAt = DateTime.UtcNow,
            ExpiresAt = DateTime.UtcNow + TimeSpan.FromDays(Int32.Parse(_conf["SecSettings:RefreshDurationDays"]!))
        });
        return tokens;
    }

    public async Task<Dictionary<string, string>> LoginAsync(LoginFormDTO lfDTO)
    {
        User? user = await _userRepo.GetUserByUsernameAsync(lfDTO.Username!);
        if (user == null) throw new UserDoesntExistException("There's no such user");
        if (new PasswordHasher<User>().VerifyHashedPassword(user, user.PasswordHash, lfDTO.Password!)
        == PasswordVerificationResult.Failed) throw new PasswordCheckFailedException("Password check failed.");
        return await GenerateTokensAsync(user);
    }


    public async Task<Dictionary<string, string>> AttemptRefreshAsync(string refreshToken)
    {
        var hasher = SHA512.Create();
        var token = Convert.ToBase64String(hasher.ComputeHash(
                    Encoding.ASCII.GetBytes(refreshToken)));
        User? user = await _refreshRepo
                        .GetUserByRefreshTokenAsync(
                            token) ?? throw new RefreshFailedException("Couldn't refresh the tokens");
        await _refreshRepo.RemoveTokenAsync(token);
        return await GenerateTokensAsync(user, true);
    }

    public async Task<User?> AuthenticateUserAsync(string token)
    {
        var handler = new JwtSecurityTokenHandler();
        var jwtSecurityToken = handler.ReadJwtToken(token);
        var claims = jwtSecurityToken.Claims;
        var UserId = claims.First(claim => claim.Type == "nameid").Value;
        return await _userSvc.GetUserByIdAsync(long.Parse(UserId));
    }

    public async Task ClearTokensAsync(string accessToken, string refreshToken)
    {
        User? user = await AuthenticateUserAsync(accessToken);
        await _refreshRepo.RemoveTokenAsync(refreshToken);
        var KeyName = $"Revoked_{accessToken}";
        await _redis.SetStringAsync(KeyName,
                                    accessToken,
                                    TimeSpan.FromMinutes(Int32.Parse(_conf["SecSettings:TokenDurationMinutes"]!)));
    }

    public async Task ClearExpiredTokensAsync()
    {
        await _refreshRepo.ClearExpiredTokensAsync();
    }


    public async Task<string[]> GetAdministratorAsync()
    {
        User? admin = await _userRepo.CheckIfExists("administrator");
        if (admin != null) return ["administrator", null];
        var Password = Guid.NewGuid().ToString();
        await RegisterAsync(new RegFormDTO()
        {
            Name = "Admin",
            Username = "administrator",
            Email = "admin@admin.admin",
            Password = Password
        }, true);
        return ["administrator", Password];
    }
}
