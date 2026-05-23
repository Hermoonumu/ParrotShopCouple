using Microsoft.AspNetCore.JsonPatch;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Application.Mappers;
using ParrotShopBackend.Domain;
using ParrotShopBackend.Infrastructure.Repos;

namespace ParrotShopBackend.Application.Services;


public class ItemService(IItemRepository _itemRepo) : IItemService
{
    public async Task CreateNewItemAsync(NewItemDTO iDTO)
    {
        Item item = ItemMapper.FromItemDTO(iDTO);
        await _itemRepo.AddAsync(item);
    }

    public async Task<List<Item>> GetAllItemsAsync(int page = 0, bool ignoreSoftDelFilter = false)
    {
        return await _itemRepo.GetAllItemsAsync(page, ignoreSoftDelFilter);
    }

    public Task<List<Item>> GetItemsByCategoryAsync(long CategoryId, int page = 0)
    {
        return _itemRepo.GetItemsByCategoryAsync(CategoryId, page);
    }

    public async Task RemoveItemAsync(long Id)
    {
        await _itemRepo.RemoveAsync(Id);
    }

    public async Task<Item?> RestoreItemAsync(long ItemId)
    {
        return await _itemRepo.RestoreItemAsync(ItemId);
    }

    public async Task<List<Item>> SearchItemsAsync(string query, int page = 0)
    {
        return await _itemRepo.SearchItemsAsync(query, page);
    }

    public async Task<List<string>> SearchSuggestionsAsync(string query)
    {
        return await _itemRepo.GetSearchSuggestionsAsync(query);
    }

    public async Task SoftDeleteItemAsync(long ItemId)
    {
        await _itemRepo.SoftDeleteItemAsync(ItemId);
    }

    public async Task<bool> UpdateItemAsync(long Id, JsonPatchDocument<Item> patchDoc)
    {
        Item? item = await _itemRepo.GetItemByIdAsync(Id);
        if (item is null) return true;
        bool hasError = false;
        patchDoc.ApplyTo(item,
                            onError =>
                            {
                                hasError = true;
                            });
        if (!hasError) { await _itemRepo.UpdateItemAsync(); return false; }
        else return true;
    }
}