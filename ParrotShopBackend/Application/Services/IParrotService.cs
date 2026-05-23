using Microsoft.AspNetCore.JsonPatch;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.Services;


public interface IParrotService
{
    public Task CreateNewParrotAsync(NewParrotDTO npDTO);
    public Task RemoveParrotAsync(long Id);
    public Task<string> UpdateParrotAsync(long Id, JsonPatchDocument<Parrot> patchDoc);
    public Task SoftDeleteParrotAsync(long ParrotId);
    public Task<Parrot?> RestoreParrotAsync(long ParrotId);
    public Task<List<Parrot>> GetAllParrotsAsync(int page = 0, bool ignoreSoftDelFilter = false);
    public Task AddTraitToParrotAsync(long Id, TraitsDTO tDTO);
    public Task<Parrot?> GetParrotByIdAsync(long Id, bool includeTraits = false, bool woTracking = false);
    public Task<List<Parrot>> FilterParrotsAsync(ParrotFilterDTO filterDTO);


}

