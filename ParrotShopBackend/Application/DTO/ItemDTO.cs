namespace ParrotShopBackend.Application.DTO;

public class ItemDTO
{
    public long? Id { get; set; }
    public string? Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? ImageUrl { get; set; }
    public long? CategoryId { get; set; }
    public double? Discount { get; set; }
    public bool IsDeleted { get; set; } = false; //soft delete
}