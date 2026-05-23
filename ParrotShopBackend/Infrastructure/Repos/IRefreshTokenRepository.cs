using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;


public interface IRefreshTokenRepository
{
    public Task AddTokenAsync(RefreshToken token);
    public Task RemoveAllUserTokensAsync(long UserId);
    public Task RemoveTokenAsync(string token);
    public Task<List<RefreshToken>> GetAllUserTokensAsync(long UserId);
    public Task<User?> GetUserByRefreshTokenAsync(string refreshToken);

    public Task ClearExpiredTokensAsync();

}