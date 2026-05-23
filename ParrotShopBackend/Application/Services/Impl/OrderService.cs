using Microsoft.AspNetCore.JsonPatch;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Application.Exceptions;
using ParrotShopBackend.Application.Extensions;
using ParrotShopBackend.Application.Mappers;
using ParrotShopBackend.Domain;
using ParrotShopBackend.Infrastructure.Repos;

namespace ParrotShopBackend.Application.Services;



public class OrderService(IOrderRepository _ordRepo) : IOrderService
{
    public async Task ChangeOrderStatusAsync(string status, long orderId)
    {
        Order? order = await _ordRepo.GetOrderByIdAsync(orderId);
        if (order is null) throw new ItemDoesntExistException("Order doesn't exist");
        order.Status = status;
        await _ordRepo.SaveChangedOrderAsync();
    }

    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _ordRepo.GetAllOrdersAsync();
    }

    public async Task<List<Order>> GetPendingOrdersAsync()
    {
        return await _ordRepo.GetPendingOrdersAsync();
    }

    public async Task<bool> PatchOrderAsync(long Id, JsonPatchDocument<Order> patchDoc)
    {
        Order? ord = await _ordRepo.GetOrderByIdAsync(Id);
        if (ord is null) return true;
        bool hasError = false;
        patchDoc.ApplyTo(ord,
                            onError =>
                            {
                                hasError = true;
                            });
        if (!hasError) { await _ordRepo.SaveChangedOrderAsync(); return false; }
        else return true;
    }
}