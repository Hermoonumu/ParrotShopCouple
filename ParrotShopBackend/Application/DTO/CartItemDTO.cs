namespace ParrotShopBackend.Application.DTO;

using ParrotShopBackend.Domain;




public class CartItemDTO
{
    public long? Id { get; set; }
    public long? CartId { get; set; }
    public long? ItemId { get; set; }
    public Item? Item { get; set; }
    public int? Qty { get; set; }
}