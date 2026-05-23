using Microsoft.EntityFrameworkCore;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Infrastructure.Repos;


public class RefreshTokenRepository(ShopContext _db) : IRefreshTokenRepository
{
    public async Task AddTokenAsync(RefreshToken token)
    {
        await _db.RefreshTokens.AddAsync(token);
        await _db.SaveChangesAsync();
    }

    public async Task ClearExpiredTokensAsync()
    {
        await _db.RefreshTokens.Where(tk => tk.ExpiresAt < DateTime.UtcNow).ExecuteDeleteAsync();
    }

    public async Task<List<RefreshToken>> GetAllUserTokensAsync(long UserId)
    {
        return await _db.RefreshTokens.Where(tk => tk.UserID == UserId).ToListAsync();
    }

    public async Task<User?> GetUserByRefreshTokenAsync(string refreshToken)
    {
        long userId = await _db.RefreshTokens.Where(tk => tk.Token == refreshToken).Select(tk => tk.UserID).FirstOrDefaultAsync();
        return await _db.Users.Where(u => u.Id == userId).FirstOrDefaultAsync();
    }

    public async Task RemoveAllUserTokensAsync(long UserId)
    {
        await _db.RefreshTokens.Where(tk => tk.UserID == UserId).ExecuteDeleteAsync();
    }

    public async Task RemoveTokenAsync(string token)
    {
        await _db.RefreshTokens.Where(tk => tk.Token == token).ExecuteDeleteAsync();
    }
}