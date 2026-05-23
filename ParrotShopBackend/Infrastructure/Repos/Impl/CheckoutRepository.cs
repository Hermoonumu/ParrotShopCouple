using Microsoft.EntityFrameworkCore;
using ParrotShopBackend.Application.Exceptions;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;




public class CheckoutRepository(ShopContext _db) : ICheckoutRepository
{ }