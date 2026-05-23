using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.Mappers;



public static class UserMapper
{
    public static User FromRegFormDTO(RegFormDTO dto)
    {
        User user = new User()
        {
            Name = dto.Name ?? string.Empty,
            Username = dto.Username ?? string.Empty,
            Email = dto.Email,
            Role = Role.User,
            Phone = dto.Phone,
            ShippingAddress = dto.ShippingAddress
        };

        return user;
    }
}
