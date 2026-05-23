using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParrotShopBackend.Domain;


public class Item
{
    [Key]
    public long Id { get; set; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    public string? Description { get; set; }
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Price { get; set; }
    public double Discount { get; set; } = 0;
    public string? ImageUrl { get; set; }

    public ItemCategory? Category { get; set; } //link the category to the item
    public long? CategoryId { get; set; }

    public bool IsDeleted { get; set; } = false; //soft delete
}



