using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Application.Services;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.API;

[ApiController]
[Route("api/checkout")]
public class CheckoutController(ICheckoutService _chkSvc, IUserService _userSvc) : ControllerBase
{
    [Authorize]
    [HttpPost]
    public async Task<IActionResult> Checkout()
    {
        User user = (await _userSvc.GetUserByToken(User, true))!;
        await _chkSvc.CheckoutAsync(user);
        return Ok();
    }

}