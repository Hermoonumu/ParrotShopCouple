namespace ParrotShopBackend.Application.DTO;

using ParrotShopBackend.Domain;
using ParrotShopBackend.Application.DTO;


public class CartDTO
{
    public long? Id { get; set; }
    public long? UserId { get; set; }

    public List<CartItem> CartItems { get; set; } = new();
}