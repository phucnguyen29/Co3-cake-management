using AuthAPI.DTOs;
using AuthAPI.Models;
using AutoMapper;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPI.Repositories.Interfaces;
using WebAPI.Services.Interfaces;
namespace WebAPI.Services
{
    public class AuthService : IAuthService
    {
        private readonly IAuthRepository _authRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;

        public AuthService(IAuthRepository authRepository, IMapper mapper, IConfiguration configuration)
        {
            _authRepository = authRepository;
            _mapper = mapper;
            _configuration = configuration;
        }


        public async Task<string> RegisterAsync(RegisterDTO dto)
        {
            // Check if username already exists
            var existingUser = await _authRepository.GetByUsernameAsync(dto.Username);
            if (existingUser != null)
            {
                throw new InvalidOperationException("Tên đăng nhập đã tồn tại.");
            }

            // Create new User from DTO
            var user = _mapper.Map<User>(dto);

            // Hash password using BCrypt.Net
            user.Password = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            // Save User to database
            await _authRepository.AddAsync(user);

            // Return
            return "User registered successfully";
        }






        public async Task<string> LoginAsync(LoginDTO dto)
        {
            var user = await _authRepository.GetByUsernameAsync(dto.Username);
            // nếu user = null hoặc không đúng password thì trả về lỗi
            if (user == null || !BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))
            {
                throw new UnauthorizedAccessException("Sai tên đăng nhập hoặc mật khẩu chưa đúng.");
            }
            var token = await GenerateJwtTokenAsync(user);
            return token;
        }


        public Task<string> GenerateJwtTokenAsync(User user)
        {
            // Lấy key từ appsettings.json
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            // Tạo Claims
            var claims = new[]
            {
        new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
        new Claim(ClaimTypes.Name, user.UserName),
        new Claim(ClaimTypes.Role, user.Role)
    };

            // Ký token
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Tạo token
            var token = new JwtSecurityToken(
                issuer: _configuration["Jwt:Issuer"],
                audience: _configuration["Jwt:Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // Thời gian sống token
                signingCredentials: creds
            );
            // Chuyển token thành string
            var tokenString = new JwtSecurityTokenHandler().WriteToken(token);
            return Task.FromResult(tokenString);
        }
    }




}
