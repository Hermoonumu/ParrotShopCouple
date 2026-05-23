using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.Services;



public interface ICategoryService
{
    public Task CreateNewCategoryAsync(NewCatDTO ncDTO);
    public Task RemoveCategoryAsync(long Id);
    public Task SetCategoryNameAsync(long CategoryId, string Name);
    public Task SetCategoryDescriptionAsync(long CategoryId, string Description);
    public Task<List<ItemCategory>> GetAllCategoriesAsync();
    public Task AddItemToCategoryAsync(long ItemId, long CategoryId);
    public Task RemoveItemFromCategoryAsync(long ItemId, long CategoryId);


}
