using Microsoft.EntityFrameworkCore;
using ParrotShopBackend.Application.Exceptions;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;




public class CartRepository(ShopContext _db) : ICartRepository
{
    public async Task<List<CartItem>> GetCartItemsAsync(long CartId)
    {
        Cart? cart = await _db.Carts.Where(c => c.Id == CartId).FirstOrDefaultAsync();
        if (cart is null) throw new ItemDoesntExistException("Cart doesn't exist");
        return cart.CartItems;
    }

    public async Task AddCartAsync(Cart cart)
    {
        await _db.Carts.AddAsync(cart);
        await _db.SaveChangesAsync();
    }

    public async Task SaveCartChangeAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task<Cart?> GetCartAsync(long CartId)
    {
        return await _db.Carts.Include(c => c.CartItems).Where(c => c.Id == CartId).FirstOrDefaultAsync();
    }
}