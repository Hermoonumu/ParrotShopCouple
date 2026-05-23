using Microsoft.AspNetCore.JsonPatch;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Application.Exceptions;
using ParrotShopBackend.Application.Extensions;
using ParrotShopBackend.Application.Mappers;
using ParrotShopBackend.Domain;
using ParrotShopBackend.Infrastructure.Repos;

namespace ParrotShopBackend.Application.Services;



public class ParrotService(IParrotRepository _parrotRepo, RedisCacheExtension _redis) : IParrotService
{
    public async Task AddTraitToParrotAsync(long Id, TraitsDTO tDTO)
    {
        Parrot? parrot = await _parrotRepo.GetParrotByIdAsync(Id, true);
        if (parrot is null) throw new ItemDoesntExistException("Parrot doesn't exist");
        await _redis.RemoveParrotFromCacheAsync(parrot);
        await _parrotRepo.RemoveTraitAsync(parrot);
        parrot.Traits = TraitsMapper.FromTraitsDTO(tDTO);
        await _parrotRepo.UpdateParrotAsync();
        await _redis.AddParrotToCacheAsync(parrot);
    }

    public async Task CreateNewParrotAsync(NewParrotDTO npDTO)
    {
        Parrot parrot = ParrotMapper.FromParrotDTO(npDTO);
        await _parrotRepo.AddAsync(parrot);
        await _redis.AddParrotToCacheAsync(parrot);
    }

    public async Task<List<Parrot>> FilterParrotsAsync(ParrotFilterDTO filterDTO)
    {
        List<long> Ids = await _redis.ApplyFilterAsync(filterDTO);
        List<Parrot?> parrots = [];
        foreach (long Id in Ids)
        {
            parrots.Add(await _parrotRepo.GetParrotByIdAsync(Id, true));
        }
        return parrots;

    }

    public async Task<List<Parrot>> GetAllParrotsAsync(int page = 0, bool ignoreSoftDelFilter = false)
    {
        return await _parrotRepo.GetAllParrotsAsync(page, ignoreSoftDelFilter);
    }

    public Task<Parrot?> GetParrotByIdAsync(long Id, bool includeTraits = false, bool woTracking = false)
    {
        return _parrotRepo.GetParrotByIdAsync(Id, includeTraits, woTracking);
    }

    public async Task RemoveParrotAsync(long Id)
    {
        await _redis.RemoveParrotFromCacheAsync(await _parrotRepo.GetParrotByIdAsync(Id, true));
        await _parrotRepo.RemoveAsync(Id);
    }

    public async Task<Parrot?> RestoreParrotAsync(long ParrotId)
    {
        await _redis.AddParrotToCacheAsync(await _parrotRepo.GetParrotByIdAsync(ParrotId, true));
        return await _parrotRepo.RestoreParrotAsync(ParrotId);
    }

    public async Task SoftDeleteParrotAsync(long ParrotId)
    {
        await _redis.RemoveParrotFromCacheAsync(await _parrotRepo.GetParrotByIdAsync(ParrotId, true));
        await _parrotRepo.SoftDeleteParrotAsync(ParrotId);
    }

    public async Task<string> UpdateParrotAsync(long Id, JsonPatchDocument<Parrot> patchDoc)
    {
        Parrot? oldParrot = await _parrotRepo.GetParrotByIdAsync(Id, true, true);
        Parrot? parrotToUpdate = await _parrotRepo.GetParrotByIdAsync(Id, true, true);
        if (oldParrot is null) return "Parrot doesn't exist";
        string hasError = null;
        patchDoc.ApplyTo(parrotToUpdate, onError => { hasError = onError.ToString(); });

        if (hasError is null)
        {
            await _parrotRepo.UpdateParrotAsync();
            if (parrotToUpdate.IsDeleted)
            {
                await _redis.RemoveParrotFromCacheAsync(parrotToUpdate);
            }
            else
            {
                await _redis.RemoveParrotFromCacheAsync(parrotToUpdate);
                await _redis.AddParrotToCacheAsync(parrotToUpdate);
            }
            return null;
        }

        return hasError;
    }
}
