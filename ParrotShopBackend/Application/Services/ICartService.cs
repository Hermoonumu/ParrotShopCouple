using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.Services;


public interface ICartService
{
    public Task<CartItem> PutItemToCartAsync(long itemId, int qty, User user);
    public Task RemoveItemFromCartAsync(long itemId, User user);
    public Task<CartDTO> GetCartItemsAsync(User user);

    public Task<Cart> NewCartAsync();

    public Task ChangeQtyAsync(long itemId, int newQty, User user);

}