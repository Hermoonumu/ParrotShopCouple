using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.Services;



public interface ICheckoutService
{
    public Task CheckoutAsync(User user);
}