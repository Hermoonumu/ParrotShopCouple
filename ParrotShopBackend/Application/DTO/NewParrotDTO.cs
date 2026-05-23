using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.DTO;


public class NewParrotDTO
{
    public required string Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public double? Discount { get; set; } = 0;
    public string? ImageUrl { get; set; }
    public byte Age { get; set; }
    public List<Color> ColorType { get; set; }
    public Gender GenderType { get; set; }
    public Species SpeciesType { get; set; }

}