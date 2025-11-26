using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using WebClient.Data;
using WebClient.DTOs;
using WebClient.Models;
using WebClient.Service.Interfaces;

namespace WebClient.Controllers
{
    [Route("[controller]/[action]")]
    public class AuthController : Controller
    {
        private readonly IAuthService _service;

        public AuthController(IAuthService service)
        {
            _service = service;
        }


        // GET: AuthController/Login
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        // POST: AuthController/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginDTO dto)
        {
            try
            {
                bool loginSuccess = await _service.LoginAsync(dto);

                if (loginSuccess)
                {
                    // ✅ Lấy token từ Session (đã được lưu trong AuthService)
                    var token = HttpContext.Session.GetString("authToken");

                    if (!string.IsNullOrEmpty(token))
                    {
                        // ✅ Gửi token sang View thông qua TempData để lưu vào localStorage
                        TempData["Token"] = token;
                    }

                    // ✅ Chuyển hướng đến trang Game List (JS Client)
                    return RedirectToAction("Index", "Products");
                }

                // ❌ Nếu login thất bại
                ModelState.AddModelError("", "Tên đăng nhập hoặc mật khẩu không chính xác.");
                return View(dto);
            }
            catch (UnauthorizedAccessException ex)
            {
                ModelState.AddModelError("", ex.Message);
                return View();
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi hệ thống: " + ex.Message);
                return View();
            }
        }



        // POST: AuthController/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            try
            {
                // ✅ Xóa token trong Session
                HttpContext.Session.Remove("authToken");

                // ✅ Xóa username cookie (nếu có)
                if (HttpContext.Request.Cookies.ContainsKey("Username"))
                {
                    HttpContext.Response.Cookies.Delete("Username");
                }

                // ✅ Chuyển hướng về trang đăng nhập
                return RedirectToAction("Login", "Auth");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đã xảy ra lỗi khi đăng xuất: " + ex.Message);
                return RedirectToAction("Index", "Home");
            }
        }


        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        // POST: AuthController/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterDTO dto)
        {
            if (!ModelState.IsValid)
                return View(dto);

            try
            {
                await _service.RegisterAsync(dto); // Gọi API AuthAPI đăng ký tài khoản
                TempData["Success"] = "Đăng ký thành công! Vui lòng đăng nhập.";
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Đăng ký thất bại: " + ex.Message);
                return View(dto);
            }
        }
    }
}
