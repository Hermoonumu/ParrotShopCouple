using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.Mappers;



public static class OrderMapper
{
    public static OrderItem CartItemToOrderItem(CartItem item)
    {
        return new OrderItem()
        {
            LinkedItemId = item.Item.Id,
            Name = item.Item.Name,
            Description = item.Item.Description,
            PriceAtOrderTime = item.Item.Price,
            ImageUrl = item.Item.ImageUrl,
            Qty = item.Qty
        };

    }

    public static OrderDTO FromOrder(Order order)
    {
        OrderDTO dto = new OrderDTO()
        {
            Id = order.Id,
            UserId = long.Parse(order.UserId.ToString()),
            Status = order.Status,
            Timestamp = order.Timestamp,
            ShippingAddress = order.ShippingAddress,
            Total = order.Total,
            OrderItems = order.OrderItems
        };
        return dto;
    }
}