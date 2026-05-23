using Microsoft.AspNetCore.JsonPatch;
using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;


public interface ICartItemRepository
{
    public Task AddAsync(CartItem cartItem);
    public Task RemoveAsync(long Id);

}