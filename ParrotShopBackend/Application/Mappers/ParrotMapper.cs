using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.Mappers;



public static class ParrotMapper
{
    public static Parrot FromParrotDTO(NewParrotDTO npDTO)
    {
        Color color = 0;
        foreach(Color c in npDTO.ColorType) color |= c;
        
        Parrot parrot = new Parrot()
        {
            Name = npDTO.Name,
            Description = npDTO.Description,
            Price = npDTO.Price??0,
            Discount = npDTO.Discount??0,
            ImageUrl = npDTO.ImageUrl,
            Age = npDTO.Age,
            ColorType = color,
            GenderType = npDTO.GenderType,
            SpeciesType = npDTO.SpeciesType
        };
        return parrot;
    }
}