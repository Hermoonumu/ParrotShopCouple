using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Application.Services;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.API;

[ApiController]
[Route("/api/items")]
public class ItemController(IItemService _itemSvc) : ControllerBase
{
    [HttpPost]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> CreateNewItem([FromBody] NewItemDTO iDTO)
    {
        await _itemSvc.CreateNewItemAsync(iDTO);
        return Ok();
    }
    [HttpPatch("{Id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> UpdateItem([FromRoute] long Id, [FromBody] JsonPatchDocument<Item> patchDoc)
    {
        bool hasError = await _itemSvc.UpdateItemAsync(Id, patchDoc);
        if (hasError) return BadRequest();
        else return Ok();
    }

    [HttpDelete("{Id}")]
    [Authorize(Policy = "Admin")]
    public async Task<IActionResult> DeleteItem([FromRoute] long Id, [FromBody] DelConfDTO dcDTO)
    {
        if (dcDTO.Confirmation != "I know it's irreversible")
            return BadRequest(new
            {
                Message = "If you acknowledge that you wish to do that " +
                            "pass a string into body saying \"I know it's irreversible\". Otherwise consider soft delete."
            });
        await _itemSvc.RemoveItemAsync(Id);
        return Ok();
    }

    [HttpGet]
    public async Task<IActionResult> GetAllItems([FromQuery] int page = 0, bool ignoreSoftDelFilter = false)
    {
        return Ok(await _itemSvc.GetAllItemsAsync(page, ignoreSoftDelFilter));
    }

    [HttpGet("search")]
    public async Task<IActionResult> SearchItems([FromQuery] string query, int page = 0)
    {
        return Ok(await _itemSvc.SearchItemsAsync(query, page));
    }

    [HttpGet("searchSuggestions")]
    public async Task<IActionResult> SearchSuggestions([FromQuery] string query)
    {
        return Ok(await _itemSvc.SearchSuggestionsAsync(query));
    }


    [HttpGet("byCategory/{Id}")]
    public async Task<IActionResult> GetItemsByCategory([FromRoute] long Id, int page = 0)
    {
        return Ok(await _itemSvc.GetItemsByCategoryAsync(Id, page));
    }

}