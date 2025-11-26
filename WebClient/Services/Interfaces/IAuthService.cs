using WebClient.DTOs;

namespace WebClient.Service.Interfaces
{
    public interface IAuthService
    {
        Task<bool> RegisterAsync(RegisterDTO dto);
        Task<bool> LoginAsync(LoginDTO dto);
        Task<bool> LogoutAsync();
    }

}
