using System.ComponentModel.DataAnnotations;

namespace ParrotShopBackend.Domain;

public class RevokedJWT
{
    [Key]
    public string Token { get; set; }

}