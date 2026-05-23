using Microsoft.EntityFrameworkCore;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Application.Mappers;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;


public class ItemRepository(ShopContext _db) : IItemRepository
{
    public async Task AddAsync(Item item)
    {
        await _db.Items.AddAsync(item);
        await _db.SaveChangesAsync();
    }

    public Task<List<Item>> GetAllItemsAsync(int page = 0, bool ignoreSoftDelFilter = false)
    {
        if (ignoreSoftDelFilter) return _db.Items.IgnoreQueryFilters().Skip(page * 10).Take(10).ToListAsync();
        return _db.Items.Skip(page * 10).Take(10).ToListAsync();
    }

    public async Task<Item?> GetItemByIdAsync(long Id)
    {
        Item? item = await _db.Items.FindAsync(Id);
        return item;
    }

    public async Task<List<Item>> GetItemsByCategoryAsync(long CategoryId, int page = 0)
    {
        ItemCategory? cat = await _db.ItemCategories.Include(c => c.Items)
                                                    .Where(c => c.Id == CategoryId)
                                                    .Skip(page * 10)
                                                    .Take(10)
                                                    .FirstOrDefaultAsync();
        if (cat is null) return new List<Item>();
        return cat.Items;

    }

    public async Task<List<string>> GetSearchSuggestionsAsync(string query)
    {
        return await _db.Items
                        .Where(i => i.Name.ToLower().StartsWith(query.ToLower()))
                        .Select(i => i.Name)
                        .Take(10)
                        .ToListAsync();
    }

    public async Task RemoveAsync(long Id)
    {
        var item = await _db.Items.IgnoreQueryFilters()
                                    .Where(i => i.Id == Id)
                                    .FirstOrDefaultAsync();
        if (item is null) return;
        _db.Items.Remove(item);
        await _db.SaveChangesAsync();
    }

    public async Task<Item?> RestoreItemAsync(long ItemId)
    {
        Item? item = await _db.Items.FindAsync(ItemId);
        if (item == null) return null!;
        item!.IsDeleted = false;
        await _db.SaveChangesAsync();
        return item;
    }

    public async Task<List<Item>> SearchItemsAsync(string query, int page = 0)
    {
        List<Item> items = await _db.Items
                                    .Include(i => (i as Parrot).Traits)
                                    .Where(i => i.Name.ToLower()
                                    .Contains(query.ToLower()))
                                    .Skip(page * 10)
                                    .Take(10)
                                    .ToListAsync();

        return items;
    }

    public async Task SoftDeleteItemAsync(long ItemId)
    {
        Item? item = _db.Items.Where(i => i.Id == ItemId).FirstOrDefault();
        if (item is not null)
        {
            item.IsDeleted = true;
            await _db.SaveChangesAsync();
        }
    }

    public async Task UpdateItemAsync()
    {
        await _db.SaveChangesAsync();
    }
}