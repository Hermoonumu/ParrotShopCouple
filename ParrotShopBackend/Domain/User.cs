using System.ComponentModel.DataAnnotations;

namespace ParrotShopBackend.Domain;


public class User
{
    [Required]
    [Key]
    public long Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    [Required]
    [StringLength(100)]
    public string Username { get; set; }
    [Required]
    public string PasswordHash { get; set; }
    [StringLength(100)]
    public string? Email { get; set; }
    [StringLength(30)]
    public string? Phone { get; set; }
    [StringLength(200)]
    public string? ShippingAddress { get; set; }
    [Required]
    public Role Role { get; set; }
    public Cart? Cart { get; set; } //we don't need a cart for user to exist
    public List<Order> Orders { get; set; }
}


public enum Role
{
    User,
    Admin
}