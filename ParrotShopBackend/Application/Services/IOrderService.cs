using Microsoft.AspNetCore.JsonPatch;
using ParrotShopBackend.Domain;

public interface IOrderService
{
    public Task<List<Order>> GetAllOrdersAsync();
    public Task<bool> PatchOrderAsync(long Id, JsonPatchDocument<Order> patchDoc);
    public Task ChangeOrderStatusAsync(string status, long orderId);
    public Task<List<Order>> GetPendingOrdersAsync();

}