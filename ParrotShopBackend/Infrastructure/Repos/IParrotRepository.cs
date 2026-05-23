using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;

public interface IParrotRepository
{
    public Task AddAsync(Parrot parrot);
    public Task<Parrot?> GetParrotByIdAsync(long Id, bool IncludeTraits = false, bool ignoreSoftDelFilter = false, bool woTracking = false);
    public Task RemoveAsync(long Id);
    public Task<Parrot?> RestoreParrotAsync(long ParrotId);
    public Task SoftDeleteParrotAsync(long ParrotId);
    public Task UpdateParrotAsync();
    public Task<List<Parrot>> GetAllParrotsAsync(int page = 0, bool ignoreSoftDelFilter = false);
    public Task RemoveTraitAsync(Parrot parrot);
}