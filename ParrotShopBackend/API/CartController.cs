using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Application.Exceptions;
using ParrotShopBackend.Application.Services;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.API;

[ApiController]
[Route("api/cart")]
public class CartController(ICartService _cartSvc, IUserService _userSvc) : ControllerBase
{
    [HttpGet]
    [Authorize]
    public async Task<IActionResult> GetCart()
    {
        User? usr = await _userSvc.GetUserByToken(User, true);
        return Ok(await _cartSvc.GetCartItemsAsync(usr!));
    }

    [HttpPost]
    [Authorize]
    public async Task<IActionResult> PutToCart([FromQuery] long ItemId, int qty)
    {
        User? usr = await _userSvc.GetUserByToken(User, true);
        return Ok(await _cartSvc.PutItemToCartAsync(ItemId, qty, usr!));
    }

    [HttpDelete()]
    [Authorize]
    public async Task<IActionResult> RemoveFromCart([FromQuery] long ItemId)
    {
        User? user = await _userSvc.GetUserByToken(User, true);
        try
        {
            await _cartSvc.RemoveItemFromCartAsync(ItemId, user!);
        }
        catch (ItemDoesntExistException e)
        {
            return BadRequest(new
            {
                Message = e.Message
            });
        }

        return Ok();
    }

    [HttpDelete("ClearAll")]
    [Authorize]
    public async Task<IActionResult> ClearCart()
    {
        await _userSvc.ClearCart(await _userSvc.GetUserByToken(User, true)!);
        return Ok();
    }

    [HttpPatch]
    [Authorize]
    public async Task<IActionResult> ChangeQty([FromQuery] long itemId, int qty)
    {
        User user = await _userSvc.GetUserByToken(User, true);
        try
        {
            await _cartSvc.ChangeQtyAsync(itemId, qty, user);
        }
        catch (ItemDoesntExistException e)
        {
            return BadRequest(new
            {
                Message = e.Message
            });
        }
        return Ok();
    }
}
