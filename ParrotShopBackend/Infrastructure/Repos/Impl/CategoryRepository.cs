using Microsoft.EntityFrameworkCore;
using ParrotShopBackend.Application.Exceptions;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;




public class CategoryRepository(ShopContext _db) : ICategoryRepository
{
    public async Task AddAsync(ItemCategory category)
    {
        await _db.ItemCategories.AddAsync(category);
        await _db.SaveChangesAsync();
    }

    public async Task AddItemToCategory(long ItemId, long CategoryId)
    {
        Item? itm = await _db.Items.Where(i => i.Id == ItemId).LastOrDefaultAsync();
        if (itm is null) throw new ItemDoesntExistException("Item doesn't exist");
        ItemCategory? cat = await _db.ItemCategories.Where(c => c.Id == CategoryId).LastOrDefaultAsync();
        if (cat is null) throw new ItemDoesntExistException("Category doesn't exist");
        itm.Category = cat;
        cat.Items.Add(itm);
        await _db.SaveChangesAsync();
    }

    public async Task<List<ItemCategory>> GetAllCategoriesAsync()
    {
        return await _db.ItemCategories.ToListAsync();
    }

    public async Task RemoveAsync(long Id)
    {
        await _db.ItemCategories.Where(c => c.Id == Id).ExecuteDeleteAsync();
    }

    public async Task RemoveItemFromCategory(long ItemId, long CategoryId)
    {
        Item? itm = await _db.Items.Where(i => i.Id == ItemId).LastOrDefaultAsync();
        if (itm is null) throw new ItemDoesntExistException("Item doesn't exist");
        ItemCategory? cat = await _db.ItemCategories.Where(c => c.Id == CategoryId).LastOrDefaultAsync();
        if (cat is null) throw new ItemDoesntExistException("Category doesn't exist");
        itm.Category = null;
        cat.Items.Remove(itm);
        await _db.SaveChangesAsync();
        return;
    }

    public async Task SetCategoryDescriptionAsync(long CategoryId, string Description)
    {
        ItemCategory? cat = await _db.ItemCategories.Where(c => c.Id == CategoryId)
                                                    .FirstOrDefaultAsync();
        if (cat is null) throw new ItemDoesntExistException("Category doesn't exist");
        cat.Description = Description;
        await _db.SaveChangesAsync();

    }

    public async Task SetCategoryNameAsync(long CategoryId, string Name)
    {
        ItemCategory? cat = await _db.ItemCategories.Where(c => c.Id == CategoryId)
                                                    .FirstOrDefaultAsync();
        if (cat is null) throw new ItemDoesntExistException("Category doesn't exist");
        cat.Name = Name;
        await _db.SaveChangesAsync();
    }


}