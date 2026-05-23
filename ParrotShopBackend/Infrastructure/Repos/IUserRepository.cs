using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;


public interface IUserRepository
{
    public Task AddUserToDBAsync(User user);
    Task<User?> GetUserByIdAsync(long id, bool includeCart = false);
    public Task<User?> GetUserByUsernameAsync(string username);
    public Task RemoveUserAsync(User user);
    public Task<User?> CheckIfExists(string username);
    public Task ClearCart(User user);
    public Task UpdateUserAsync();
}

