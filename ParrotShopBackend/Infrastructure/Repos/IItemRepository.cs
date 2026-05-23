
using Microsoft.AspNetCore.JsonPatch;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;


public interface IItemRepository
{
    public Task AddAsync(Item item);
    public Task<Item?> GetItemByIdAsync(long Id);
    public Task RemoveAsync(long Id);
    public Task<Item?> RestoreItemAsync(long ItemId);
    public Task UpdateItemAsync();
    public Task SoftDeleteItemAsync(long ItemId);
    public Task<List<Item>> GetAllItemsAsync(int page = 0, bool ignoreSoftDelFilter = false);
    public Task<List<Item>> SearchItemsAsync(string query, int page = 0);
    public Task<List<Item>> GetItemsByCategoryAsync(long CategoryId, int page = 0);
    public Task<List<string>> GetSearchSuggestionsAsync(string query);


}

