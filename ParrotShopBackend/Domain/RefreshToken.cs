using System.ComponentModel.DataAnnotations;

namespace ParrotShopBackend.Domain;



public class RefreshToken
{
    [Key]
    public string Token { get; set; }
    [Required]
    public long UserID { set; get; }
    [Required]
    public User User { set; get; }
    [Required]
    public DateTime IssuedAt { set; get; }
    [Required]
    public DateTime ExpiresAt { set; get; }
}