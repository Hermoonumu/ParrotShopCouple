using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;



public interface ICartRepository
{
    public Task<List<CartItem>> GetCartItemsAsync(long CartId);
    public Task AddCartAsync(Cart cart);
    public Task SaveCartChangeAsync();
    public Task<Cart?> GetCartAsync(long CartId);
}
