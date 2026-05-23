using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.Mappers;



public class ItemMapper
{
    public static Item FromItemDTO(NewItemDTO iDTO)
    {
        return new Item()
        {
            Name = iDTO.Name!,
            Description = iDTO.Description,
            Price = iDTO.Price,
            ImageUrl = iDTO.ImageUrl,
            CategoryId = iDTO.CategoryId,
            IsDeleted = false
        };
    }
}