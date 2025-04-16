namespace ShopeeFoodClone.WebMvc.Administrators.Application.Interfaces;

public interface IAccountService
{
    Task<Response?> LoginAsync(LoginViewModel model);
    Task<Response?> LoginWithRefreshTokenAsync(LoginRefreshTokenRequest model);
}