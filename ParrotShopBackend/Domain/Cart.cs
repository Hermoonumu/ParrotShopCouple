using System.ComponentModel.DataAnnotations;

namespace ParrotShopBackend.Domain;


public class Cart
{
    [Key]
    [Required]
    public long Id { get; set; }
    public long? UserId { get; set; }
    public User? User { get; set; }
    public List<CartItem> CartItems { get; set; }
}

