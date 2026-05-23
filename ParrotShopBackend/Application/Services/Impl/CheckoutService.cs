using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Application.Exceptions;
using ParrotShopBackend.Application.Mappers;
using ParrotShopBackend.Domain;
using ParrotShopBackend.Infrastructure.Repos;

namespace ParrotShopBackend.Application.Services;




public class CheckoutService(ICheckoutRepository _chkRepo, IOrderRepository _ordRepo, IUserRepository _usrRepo) : ICheckoutService
{
    public async Task CheckoutAsync(User user) // Pass address from Frontend DTO
    {
        if (user.Cart == null || !user.Cart.CartItems.Any()) throw new ItemDoesntExistException("User has no items in cart");
        decimal TotalPrice = 0;
        user.Cart.CartItems.ForEach(i => TotalPrice += i.Item.Price * i.Qty);
        Order ord = new Order()
        {
            UserId = user.Id,
            User = user,
            Timestamp = DateTime.UtcNow,
            ShippingAddress = user.ShippingAddress,
            OrderItems = user.Cart.CartItems.Select(c => OrderMapper.CartItemToOrderItem(c)).ToList(),
            Total = TotalPrice

        };
        user.Orders.Add(ord);
        await _ordRepo.SaveOrderAsync(ord);
        await _ordRepo.SaveChangedOrderAsync();
        await _usrRepo.ClearCart(user);

    }
}