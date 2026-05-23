using System.ComponentModel.DataAnnotations;

namespace ParrotShopBackend.Domain;



public class Order
{
    [Key]
    public long Id { get; set; }
    public long? UserId { get; set; }
    public string? Status { get; set; } = "Processing";
    public User? User { get; set; }
    [Required]
    public DateTime Timestamp { set; get; }
    [Required]
    public string ShippingAddress { get; set; } = string.Empty;
    [Required]
    public decimal Total { get; set; }

    public List<OrderItem> OrderItems { get; set; } = new();

}