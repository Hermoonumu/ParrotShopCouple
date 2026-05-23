using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Application.Exceptions;
using ParrotShopBackend.Application.Mappers;
using ParrotShopBackend.Domain;
using ParrotShopBackend.Infrastructure.Repos;

namespace ParrotShopBackend.Application.Services;




public class CartService(ICartRepository _carRepo, ICartItemRepository _icartRepo, IItemRepository _itemRepo) : ICartService
{
    public async Task<CartDTO> GetCartItemsAsync(User user)
    {
        return CartMapper.FromCart(user.Cart!);
    }

    public async Task<CartItem> PutItemToCartAsync(long itemId, int qty, User user)
    {
        CartItem ct = new CartItem()
        {
            ItemId = itemId,
            CartId = user.Cart.Id,
            Qty = qty
        };
        user.Cart.CartItems.Add(ct);
        await _icartRepo.AddAsync(ct);
        await _carRepo.SaveCartChangeAsync();

        ct.Item = await _itemRepo.GetItemByIdAsync(itemId);
        return ct;
    }

    public async Task RemoveItemFromCartAsync(long itemId, User user)
    {
        List<CartItem> items = user!.Cart!.CartItems.FindAll(i => i.ItemId == itemId);
        if (items.Count <= 0)
        {
            throw new ItemDoesntExistException("No such item in cart");
        }
        user!.Cart!.CartItems.RemoveAll(c => c.ItemId == itemId);
        await _carRepo.SaveCartChangeAsync();
        return;
    }

    public async Task<Cart> NewCartAsync()
    {
        Cart cart = new Cart()
        {
            CartItems = new List<CartItem>()
        };
        await _carRepo.AddCartAsync(cart);
        return cart;
    }

    public async Task ChangeQtyAsync(long itemId, int newQty, User user)
    {
        if (user.Cart == null) throw new ItemDoesntExistException("User has no cart");
        Cart? cart = await _carRepo.GetCartAsync(user.Cart.Id);
        if (cart == null) return;
        var item = cart.CartItems.FirstOrDefault(i => i.ItemId == itemId);
        if (item == null) throw new ItemDoesntExistException("Item doesn't exist in cart");
        item.Qty = newQty;
        await _carRepo.SaveCartChangeAsync();
    }
}