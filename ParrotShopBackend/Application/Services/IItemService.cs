using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.Services;


public interface IItemService
{
    public Task CreateNewItemAsync(NewItemDTO iDTO);
    public Task RemoveItemAsync(long Id);
    public Task<bool> UpdateItemAsync(long Id, JsonPatchDocument<Item> patchDoc);
    public Task SoftDeleteItemAsync(long ItemId);
    public Task<Item?> RestoreItemAsync(long ItemId);
    public Task<List<Item>> GetAllItemsAsync(int page = 0, bool ignoreSoftDelFilter = false);
    public Task<List<Item>> SearchItemsAsync(string query, int page = 0);
    public Task<List<string>> SearchSuggestionsAsync(string query);
    public Task<List<Item>> GetItemsByCategoryAsync(long CategoryId, int page = 0);




}


