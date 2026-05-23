namespace ParrotShopBackend.Application.DTO;

using ParrotShopBackend.Domain;



public class UserDTO
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Username { get; set; }
    public string? Email { get; set; }
    public long? CartId { get; set; }
}