using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ParrotShopBackend.Application.Services;
using ParrotShopBackend.Domain;

[ApiController]
[Route("/api/user")]

public class UserController(IUserService _usrSvc) : ControllerBase
{
    [HttpPatch()]
    [Authorize]
    public async Task<IActionResult> UpdateItem([FromBody] JsonPatchDocument<User> patchDoc)
    {
        User? usr = await _usrSvc.GetUserByToken(User, true);
        if (usr == null) return Unauthorized();
        bool hasError = await _usrSvc.UpdateUserAsync(usr.Id, patchDoc);
        if (hasError) return BadRequest(new { message = "Either your username or email are already in use." });
        else return Ok();
    }

    [HttpDelete()]
    [Authorize]
    public async Task<IActionResult> DeleteUser()
    {
        User? usr = await _usrSvc.GetUserByToken(User, true);
        await _usrSvc.DeleteUserAsync(usr!);
        return Ok();
    }
}