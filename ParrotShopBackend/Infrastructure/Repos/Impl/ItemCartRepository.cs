using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;


public class CartItemRepository(ShopContext _db) : ICartItemRepository
{
    public async Task AddAsync(CartItem cartItem)
    {
        await _db.CartItems.AddAsync(cartItem);
        await _db.SaveChangesAsync();
    }

    public Task RemoveAsync(long Id)
    {
        throw new NotImplementedException();
    }
}