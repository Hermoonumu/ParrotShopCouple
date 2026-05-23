using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Application.Services;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.API;

[ApiController]
[Route("api/categories")]
public class CategoryController(ICategoryService _catSvc) : ControllerBase
{
    [Authorize(Policy = "Admin")]
    [HttpPost]
    public async Task<IActionResult> AddCategory([FromBody] NewCatDTO ncDTO)
    {
        await _catSvc.CreateNewCategoryAsync(ncDTO);
        return Ok();
    }

    [HttpGet]
    public async Task<List<ItemCategory>> GetAllCategories()
    {
        return await _catSvc.GetAllCategoriesAsync();
    }
}
