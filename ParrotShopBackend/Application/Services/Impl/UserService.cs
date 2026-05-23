using System.Security.Claims;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using ParrotShopBackend.Domain;
using ParrotShopBackend.Infrastructure.Repos;

namespace ParrotShopBackend.Application.Services;


public class UserService(IUserRepository _userRepo) : IUserService
{
    public async Task ClearCart(User user)
    {
        await _userRepo.ClearCart(user);
    }

    public async Task DeleteUserAsync(User user)
    {
        await _userRepo.RemoveUserAsync(user);
    }

    public async Task<User?> GetUserByIdAsync(long Id, bool includeCart = false)
    {
        return await _userRepo.GetUserByIdAsync(Id, includeCart);

    }

    public async Task<User?> GetUserByToken(ClaimsPrincipal principal, bool includeCart = false)
    {
        var userIdClaim = principal.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        if (string.IsNullOrEmpty(userIdClaim)) return null;
        return await _userRepo.GetUserByIdAsync(long.Parse(userIdClaim), includeCart);
    }

    public async Task<bool> UpdateUserAsync(long Id, JsonPatchDocument<User> patchDoc)
    {
        User? user = await _userRepo.GetUserByIdAsync(Id);
        if (user is null) return true;
        bool hasError = false;
        patchDoc.ApplyTo(user,
                            onError =>
                            {
                                hasError = true;
                            });
        try
        {
            await _userRepo.UpdateUserAsync();
        }
        catch (DbUpdateException ex)
        {
            if (ex.InnerException is PostgresException pgEx)
            {
                if (pgEx.SqlState == "23505")
                {
                    return true;
                }
            }
            throw;
        }
        return false;
    }



}