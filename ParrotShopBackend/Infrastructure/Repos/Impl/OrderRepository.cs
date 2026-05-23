using Microsoft.EntityFrameworkCore;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;


public class OrderRepository(ShopContext _db) : IOrderRepository
{
    public async Task AddOrderItemAsync(OrderItem oItem)
    {
        await _db.OrderItems.AddAsync(oItem);
    }

    public async Task<List<Order>> GetAllUserOrders(User user)
    {
        return await _db.Orders.Include(o => o.OrderItems).Where(o => o.UserId == user.Id).ToListAsync();
    }

    public Task<Order> GetOrderByIdAsync(long Id)
    {
        return _db.Orders.Include(o => o.OrderItems).Where(o => o.Id == Id).FirstOrDefaultAsync();
    }

    public async Task SaveChangedOrderAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task SaveOrderAsync(Order order)
    {
        await _db.Orders.AddAsync(order);
    }
    public async Task<List<Order>> GetAllOrdersAsync()
    {
        return await _db.Orders.Include(o => o.OrderItems).ToListAsync();
    }
    public async Task<List<Order>> GetPendingOrdersAsync()
    {
        return await _db.Orders.Include(o => o.OrderItems).Where(o => o.Status.ToLower() == "processing").ToListAsync();
    }
}
