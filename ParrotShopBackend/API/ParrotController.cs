using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Application.Exceptions;
using ParrotShopBackend.Application.Extensions;
using ParrotShopBackend.Application.Services;
using ParrotShopBackend.Domain;
namespace ParrotShopBackend.API;




[ApiController]
[Route("/api/parrots")]

public class ParrotController(IParrotService _parrotSvc, RedisCacheExtension _redisCache) : ControllerBase
{
    [HttpPost("testColourHandling")]
    public async Task<IActionResult> TestColourHandling([FromBody] List<Color> colours)
    {
        Color AllColours = default;
        foreach (Color col in colours) AllColours |= col;
        Parrot parrot = new Parrot { Name = "Chum", Price = 100 };
        parrot.ColorType = AllColours;

        return Ok(new { Msg = "WOW IT WORKED!", Parrot = parrot, AllColours });
    }

    [HttpGet("{Id}")]
    public async Task<IActionResult> GetParrot([FromRoute] long Id, [FromQuery] bool includeTraits = false)
    {
        Parrot? parrot = await _parrotSvc.GetParrotByIdAsync(Id, includeTraits);
        if (parrot is null) return NotFound();
        return Ok(parrot);
    }


    [Authorize(Policy = "Admin")]
    [HttpPost("")]
    public async Task<IActionResult> AddParrot([FromBody] NewParrotDTO npDTO)
    {
        await _parrotSvc.CreateNewParrotAsync(npDTO);
        return Ok();
    }
    [HttpPatch("{Id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> UpdateParrot([FromRoute] long Id, [FromBody] JsonPatchDocument<Parrot> patchDoc)
    {
        Parrot? untrackedParrot = await _parrotSvc.GetParrotByIdAsync(Id, true, true);
        string hasError = await _parrotSvc.UpdateParrotAsync(Id, patchDoc);
        if (hasError is not null) return BadRequest(new { message = hasError });
        Parrot parrot = (await _parrotSvc.GetParrotByIdAsync(Id)!);
        return Ok(parrot);
    }
    [HttpDelete("{Id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> DeleteParrot([FromRoute] long Id, [FromBody] DelConfDTO dcDTO)
    {
        if (dcDTO.Confirmation != "I know it's irreversible")
            return BadRequest(new
            {
                Message = "If you acknowledge that you wish to do that " +
                            "pass a string into body saying \"I know it's irreversible\". Otherwise consider soft delete."
            });
        await _parrotSvc.RemoveParrotAsync(Id);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllParrots([FromQuery] int page = 0, bool ignoreSoftDelFilter = false)
    {
        return Ok(await _parrotSvc.GetAllParrotsAsync(page, ignoreSoftDelFilter));
    }

    [HttpPost("BatchAdd")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> BatchAddParrots([FromBody] List<NewParrotDTO> npDTOs)
    {
        foreach (NewParrotDTO npDTO in npDTOs)
        {
            await _parrotSvc.CreateNewParrotAsync(npDTO);
        }
        return Ok();
    }
    [HttpPost("Traits/{Id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> AddTraitToParrot([FromRoute] long Id, [FromBody] TraitsDTO tDTO)
    {
        try
        {
            await _parrotSvc.AddTraitToParrotAsync(Id, tDTO);
        }
        catch (ItemDoesntExistException)
        {
            return NotFound();
        }
        return Ok();

    }

    [HttpGet("filter")]
    public async Task<IActionResult> FilterParrots([FromQuery] ParrotFilterDTO pfDTO)
    {
        List<Parrot> parrots = await _parrotSvc.FilterParrotsAsync(pfDTO);
        if (pfDTO.AscendingPrice.HasValue)
        {
            if (pfDTO.AscendingPrice.Value)
            {
                return Ok(parrots.OrderByDescending(p => p.Price).ToList());
            }
            else
            {
                return Ok(parrots.OrderBy(p => p.Price).ToList());
            }
        }
        return Ok(parrots);
    }

    [HttpGet("parrotRecommendation")]
    public async Task<IActionResult> ParrotRecommendation([FromQuery] ParrotFilterDTO pfDTO)
    {
        List<long> recIds = await _redisCache.GetRecommendationsAsync(pfDTO);
        if (recIds.Count == 0)
            return Ok(new List<Parrot>());

        List<Parrot> parrots = [];
        foreach (long i in recIds)
        {
            parrots.Add(await _parrotSvc.GetParrotByIdAsync(i, true));
        }

        if (pfDTO.AscendingPrice == true)
            parrots = parrots.OrderBy(p => p.Price).ToList();
        else if (pfDTO.AscendingPrice == false)
            parrots = parrots.OrderByDescending(p => p.Price).ToList();

        return Ok(parrots);
    }

}
