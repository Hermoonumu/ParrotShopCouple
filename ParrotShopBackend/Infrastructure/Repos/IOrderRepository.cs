using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;

public interface IOrderRepository
{
    public Task AddOrderItemAsync(OrderItem oItem);
    public Task SaveOrderAsync(Order order);
    public Task SaveChangedOrderAsync();
    public Task<List<Order>> GetAllUserOrders(User user);
    public Task<Order> GetOrderByIdAsync(long Id);
    public Task<List<Order>> GetAllOrdersAsync();
    public Task<List<Order>> GetPendingOrdersAsync();
}

