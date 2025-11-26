using AuthAPI.DTOs;
using AuthAPI.Models;

namespace WebAPI.Services.Interfaces
{
    public interface IAuthService
    {
        Task<string> RegisterAsync(RegisterDTO dto);
        Task<string> LoginAsync(LoginDTO dto);
        Task<string> GenerateJwtTokenAsync(User user);

    }
}
