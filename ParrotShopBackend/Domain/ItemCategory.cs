using System.ComponentModel.DataAnnotations;

namespace ParrotShopBackend.Domain;






public class ItemCategory
{
    [Key]
    public long Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    public string? Description { get; set; }
    public List<Item> Items { get; set; } = new List<Item>();

}