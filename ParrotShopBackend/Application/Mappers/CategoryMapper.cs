using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.Mappers;


public class CategoryMapper
{
    public static ItemCategory FromCategoryDTO(NewCatDTO cDTO)
    {
        return new ItemCategory()
        {
            Name = cDTO.Name!,
            Description = cDTO.Description,
        };
    }

}