using Microsoft.EntityFrameworkCore;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;


public class ParrotRepository(ShopContext _db) : IParrotRepository
{
    public async Task AddAsync(Parrot parrot)
    {
        await _db.Parrots.AddAsync(parrot);
        await _db.SaveChangesAsync();
    }

    public async Task<Parrot?> GetParrotByIdAsync(long id, bool includeTraits = false, bool ignoreSoftDelFilter = false, bool woTracking = false)
    {
        IQueryable<Parrot> query = _db.Parrots;
        if (woTracking)
        {
            query = query.AsNoTracking();
        }
        if (ignoreSoftDelFilter)
        {
            query = query.IgnoreQueryFilters();
        }
        if (includeTraits)
        {
            query = query.Include(p => p.Traits);
        }
        return await query.FirstOrDefaultAsync(p => p.Id == id);
    }

    public async Task RemoveAsync(long Id)
    {
        var parrot = await _db.Parrots.IgnoreQueryFilters()
                                    .Where(i => i.Id == Id)
                                    .FirstOrDefaultAsync();
        if (parrot is null) return;
        _db.Parrots.Remove(parrot);
        await _db.SaveChangesAsync();
    }

    public async Task<Parrot?> RestoreParrotAsync(long ParrotId)
    {
        var parrot = await _db.Parrots.FindAsync(ParrotId);
        if (parrot is null) return null;
        parrot.IsDeleted = false;
        await _db.SaveChangesAsync();
        return parrot;
    }

    public async Task SoftDeleteParrotAsync(long ParrotId)
    {
        var parrot = await _db.Parrots.FindAsync(ParrotId);
        if (parrot is null) return;
        parrot.IsDeleted = true;
        await _db.SaveChangesAsync();
    }

    public async Task UpdateParrotAsync()
    {
        await _db.SaveChangesAsync();
    }

    public async Task<List<Parrot>> GetAllParrotsAsync(int page = 0, bool ignoreSoftDelFilter = false)
    {
        if (ignoreSoftDelFilter) return await _db.Parrots.IgnoreQueryFilters().Include(p => p.Traits).Skip(page * 10).Take(10).ToListAsync();
        return await _db.Parrots.Include(p => p.Traits).Skip(page * 10).Take(10).ToListAsync();
    }

    public async Task RemoveTraitAsync(Parrot parrot)
    {
        parrot.Traits = null;
        await _db.Traits.Where(t => t.Id == parrot.TraitsId).ExecuteDeleteAsync();
    }
}