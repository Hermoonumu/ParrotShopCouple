
using ParrotShopBackend.Domain;
using ParrotShopBackend.Application.DTO;

namespace ParrotShopBackend.Application.Mappers;



public static class CartMapper
{
    public static CartDTO FromCart(Cart cart)
    {
        return new CartDTO()
        {
            Id = cart.Id,
            CartItems = cart.CartItems,
            UserId = cart.User.Id,
        };
    }
}