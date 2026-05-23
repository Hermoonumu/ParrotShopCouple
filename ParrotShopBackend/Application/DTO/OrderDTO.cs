
namespace ParrotShopBackend.Domain;



public class OrderDTO
{
    public long Id { get; set; }

    public long UserId { get; set; }
    public string Status { get; set; } = string.Empty;
    public DateTime Timestamp { set; get; }
    public string ShippingAddress { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public List<OrderItem> OrderItems { get; set; } = new();

}