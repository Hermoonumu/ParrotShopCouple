using ParrotShopBackend.Application.DTO;
using ParrotShopBackend.Domain;

namespace ParrotShopBackend.Application.Services;


public interface IAuthService
{
    public Task<Dictionary<string, string>> RegisterAsync(RegFormDTO rfDTO, bool admin = false);
    public Task<Dictionary<string, string>> LoginAsync(LoginFormDTO lfDTO);
    public Task<Dictionary<string, string>> AttemptRefreshAsync(string refreshToken);
    public Task<User?> AuthenticateUserAsync(string token);
    public Task ClearTokensAsync(string accessToken, string refreshToken);
    public Task ClearExpiredTokensAsync();
    public Task<string[]> GetAdministratorAsync();
}