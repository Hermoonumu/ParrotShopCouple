using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ParrotShopBackend.Application.Mappers;
using ParrotShopBackend.Application.Services;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.API;


[ApiController]
[Route("/api/orders")]

public class OrderController(IUserService _userSvc, IOrderService _ordSvc) : ControllerBase
{
    public class ChangeOrderStatusRequest
    {
        public string Status { get; set; }
        public long OrderId { get; set; }
    }
    [Authorize]
    [HttpGet]
    public async Task<IActionResult> GetOrders()
    {
        User user = await _userSvc.GetUserByToken(User, true);
        return Ok(user.Orders.Select(o => OrderMapper.FromOrder(o)).ToList());
    }

    [Authorize(Policy = "Admin")]
    [HttpGet("all")]
    public async Task<IActionResult> GetAllOrders()
    {
        List<Order> orders = await _ordSvc.GetAllOrdersAsync();
        orders.Sort((x, y) => y.Timestamp.CompareTo(x.Timestamp));
        return Ok(await _ordSvc.GetAllOrdersAsync());
    }

    [Authorize(Policy = "Admin")]
    [HttpPatch("")]
    public async Task<IActionResult> ChangeOrder([FromBody] ChangeOrderStatusRequest s)
    {
        await _ordSvc.ChangeOrderStatusAsync(s.Status, s.OrderId);
        return Ok();
    }

    [Authorize(Policy = "Admin")]
    [HttpGet("pending")]
    public async Task<IActionResult> GetPendingOrders()
    {
        List<Order> orders = await _ordSvc.GetPendingOrdersAsync();
        orders.Sort((x, y) => y.Timestamp.CompareTo(x.Timestamp));
        return Ok(orders);

    }



}
