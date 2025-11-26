using System.Text;
using System.Text.Json;
using WebClient.DTOs;
using WebClient.Service.Interfaces;

namespace WebClient.Service
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public AuthService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor)
        {
            _httpClient = httpClient;
            _httpClient.BaseAddress = new Uri("https://localhost:7193/api/Auth/");
            _httpContextAccessor = httpContextAccessor;
        }


        //Xử lý Login
        public async Task<bool> LoginAsync(LoginDTO dto)
        {

            var response = await _httpClient.PostAsJsonAsync("login", dto);

            if (response.IsSuccessStatusCode)
            {
                // Đọc kết quả JSON và parse trực tiếp sang model AuthResponse
                var authResult = await response.Content.ReadFromJsonAsync<AuthResponse>();

                if (authResult != null && !string.IsNullOrEmpty(authResult.Token))
                {
                    var httpContext = _httpContextAccessor.HttpContext;

                    if (httpContext != null && httpContext.Session != null)
                    {
                        // Lưu token vào Session
                        httpContext.Session.SetString("authToken", authResult.Token);

                        // Lưu Username vào Cookie
                        httpContext.Response.Cookies.Append("Username", dto.Username, new CookieOptions
                        {
                            HttpOnly = false, // Có thể set true nếu không cần client JS đọc cookie
                            Secure = true, // Nên bật true trong production với HTTPS
                            SameSite = SameSiteMode.Strict
                        });

                        return true;
                    }
                }
            }

            // Trường hợp login thất bại
            return false;
        }

        //Xử lý Logout
        public async Task<bool> LogoutAsync()
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext != null)
            {
                // Xóa token khỏi Session
                httpContext.Session.Remove("authToken");

                // Xóa Username cookie
                if (httpContext.Request.Cookies.ContainsKey("Username"))
                {
                    httpContext.Response.Cookies.Delete("Username");
                }

                return true;
            }
            return false;
        }

        //Xử lý Register
        public async Task<bool> RegisterAsync(RegisterDTO dto)
        {
            var json = JsonSerializer.Serialize(dto);
            var content = new StringContent(json, Encoding.UTF8, "application/json");

            var response = await _httpClient.PostAsync("https://localhost:7193/api/auth/register", content);
            if (!response.IsSuccessStatusCode)
            {
                var msg = await response.Content.ReadAsStringAsync();
                throw new Exception(msg);
            }
            return false;
        }
    }

    public class AuthResponse // Kết quả trả về sau khi đăng nhập thành công
    {
        public string Token { get; set; }
    }

}
