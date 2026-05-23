using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ParrotShopBackend.Domain;





public class OrderItem
{
    [Key]
    public long Id { get; set; }
    public long? LinkedItemId { set; get; } //Allow to set the og item id to null, since we might stop selling it
    public Item? LinkedItem { set; get; }
    [Required]
    [StringLength(100)]
    public string Name { get; set; }
    public string? Description { get; set; }
    [Required]
    [Column(TypeName = "decimal(18,2)")]
    public decimal PriceAtOrderTime { get; set; }
    public int Qty { get; set; }
    public string? ImageUrl { get; set; }
    public long? OrderId { get; set; }
}