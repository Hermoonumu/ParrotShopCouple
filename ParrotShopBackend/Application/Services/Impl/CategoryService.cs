using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Application.Mappers;
using ParrotShopBackend.Domain;
using ParrotShopBackend.Infrastructure.Repos;

namespace ParrotShopBackend.Application.Services;




public class CategoryService(ICategoryRepository _catRepo) : ICategoryService
{

    public async Task AddItemToCategoryAsync(long ItemId, long CategoryId)
    {
        await _catRepo.AddItemToCategory(ItemId, CategoryId);
        return;
    }

    public async Task CreateNewCategoryAsync(NewCatDTO ncDTO)
    {
        await _catRepo.AddAsync(CategoryMapper.FromCategoryDTO(ncDTO));
        return;
    }

    public async Task<List<ItemCategory>> GetAllCategoriesAsync()
    {
        return await _catRepo.GetAllCategoriesAsync();
    }

    public async Task RemoveCategoryAsync(long Id)
    {
        await _catRepo.RemoveAsync(Id);
        return;
    }

    public async Task RemoveItemFromCategoryAsync(long ItemId, long CategoryId)
    {
        await _catRepo.RemoveItemFromCategory(ItemId, CategoryId);
        return;
    }

    public async Task SetCategoryDescriptionAsync(long CategoryId, string Description)
    {
        await _catRepo.SetCategoryDescriptionAsync(CategoryId, Description);
        return;
    }

    public async Task SetCategoryNameAsync(long CategoryId, string Name)
    {
        await _catRepo.SetCategoryNameAsync(CategoryId, Name);
        return;
    }
}