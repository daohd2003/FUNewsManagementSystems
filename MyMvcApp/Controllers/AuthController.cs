using FUDTOs;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using Services;
using MyMvcApp.Utilities;

namespace MyMvcApp.Controllers
{
    public class AuthController : Controller
    {
        private readonly ISystemAccountService _authService;

        public AuthController(ISystemAccountService authService)
        {
            _authService = authService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDTO model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(model);

            var loginResponse = await _authService.Login(model);
            if (loginResponse == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt");
                return View(model);
            }

            // Lưu token vào Session
            HttpContext.Session.SetString("AccessToken", loginResponse.Token);

            // Giải mã token để lấy claims
            var claims = JwtUtils.DecodeToken(loginResponse.Token).ToList();

            // Thêm các claim cần thiết
            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

            return Url.IsLocalUrl(returnUrl) ? Redirect(returnUrl!) : RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult AccessDenied() => View();

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            HttpContext.Session.Remove("AccessToken");
            return RedirectToAction("Login", "Auth");
        }
    }
}