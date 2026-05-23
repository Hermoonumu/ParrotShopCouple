using System.Security.Claims;
using Microsoft.AspNetCore.JsonPatch;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.Services;


public interface IUserService
{
    public Task<User?> GetUserByIdAsync(long Id, bool includeCart = false);
    public Task<User?> GetUserByToken(ClaimsPrincipal principal, bool includeCart = false);
    public Task<bool> UpdateUserAsync(long Id, JsonPatchDocument<User> patchDoc);
    public Task ClearCart(User user);
    public Task DeleteUserAsync(User user);
}
