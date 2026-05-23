using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.Mappers;


public class CartItemMapper
{
    public static CartItem FromItem(Item item)
    {
        return new CartItem()
        {
            ItemId = item.Id,
            Item = item,
        };

    }
}