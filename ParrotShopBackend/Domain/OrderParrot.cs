using System.ComponentModel.DataAnnotations;

namespace ParrotShopBackend.Domain;



public class OrderParrot : OrderItem
{
    [Range(0, 100)]
    public byte Age { get; set; }
    public Color ColorType { get; set; }
    public Gender GenderType { get; set; }
    public Species SpeciesType { get; set; }
}