using System.Net.Mail;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using ParrotShopBackend.Domain;
using System.Text;
using System.Text.Json;
using System.Reflection;

namespace ParrotShopBackend.API;


[ApiController]
[Route("/api/TEST")]

public class TestController : ControllerBase
{
    [HttpGet("parrotTest")]
    public async Task<IActionResult> CheckTheParrot()
    {
        Parrot parrot = new Parrot { Name = "Chum", Price = 100 };
        parrot.ColorType = Color.Black | Color.White | Color.Green | Color.Colourful;

        return Ok(new { Message = $"The parrot has been successfully checked. {parrot.ColorType}" });
    }


    [HttpGet("DumpMyParrot")]
    public async Task<IActionResult> DumpMyParrot()
    {
        Parrot parrot = new Parrot { Name = "Chum", Price = 100 };
        List<string> propNames = (new Parrot())
                            .GetType()
                            .GetProperties()
                            .Select(p => p.Name)
                            .ToList();

        return Ok(new {Message="Here's what we have: ", propNames});
    }


    [HttpGet("oddEvenBitOp")]
    public async Task<IActionResult> OddEvenBitOp([FromQuery] int number)
    {
        return Ok(new { res = (number & 1) == 0 ? "Even" : "Odd", debug = number & 1 });
    }

    [HttpGet("AmIATeapot")]
    public async Task<IActionResult> AmIATeapot()
    {
        return StatusCode(418, new { Message = "Yes you are a teapot" });

    }
}
