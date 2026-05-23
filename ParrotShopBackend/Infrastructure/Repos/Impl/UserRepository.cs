using Microsoft.EntityFrameworkCore;
using Npgsql;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;



public class UserRepository(ShopContext _db) : IUserRepository
{
    public async Task AddUserToDBAsync(User user)
    {
        await _db.Users.AddAsync(user);
        await _db.SaveChangesAsync();
    }

    public async Task<User?> GetUserByIdAsync(long id, bool includeCart = false)
    {
        if (includeCart)
        {
            return await _db.Users
    .Include(u => u.Cart)
        .ThenInclude(c => c.CartItems)
            .ThenInclude(ci => ci.Item)
    .Include(u => u.Orders)
        .ThenInclude(o => o.OrderItems)
        .IgnoreQueryFilters()
    .Where(u => u.Id == id)
    .AsSplitQuery()
    .FirstOrDefaultAsync();
        }
        else
        {
            return await _db.Users.Where(u => u.Id == id).Include(u => u.Orders).ThenInclude(o => o.OrderItems).FirstOrDefaultAsync();
        }

    }

    public async Task<User?> GetUserByUsernameAsync(string username)
    {
        return await _db.Users.Include(u => u.Cart).Where(u => u.Username == username).FirstOrDefaultAsync();
    }

    public async Task RemoveUserAsync(User user)
    {
        await _db.Users.Where(u => u.Id == user.Id).ExecuteDeleteAsync();
        await _db.SaveChangesAsync();
    }
    public async Task<User?> CheckIfExists(string username)
    {
        return await _db.Users.FirstOrDefaultAsync(u => u.Username == username);
    }

    public async Task ClearCart(User user)
    {
        Cart? cart = await _db.Carts.Where(c => c.UserId == user.Id).FirstOrDefaultAsync();

        await _db.CartItems
        .Where(ci => ci.CartId == cart.Id)
        .ExecuteDeleteAsync();
    }

    public async Task UpdateUserAsync()
    {
        await _db.SaveChangesAsync();
    }

}