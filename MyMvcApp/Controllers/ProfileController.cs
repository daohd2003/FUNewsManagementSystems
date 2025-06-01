using FUDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Utilities;
using Services;
using System.Security.Claims;

namespace MyMvcApp.Controllers
{
    [ValidateJwtToken]
    /*[Authorize(Roles = "Staff")]*/
    public class ProfileController : Controller
    {
        private readonly ISystemAccountService _accountService;

        public ProfileController(ISystemAccountService accountService)
        {
            _accountService = accountService;
        }

        public async Task<IActionResult> Index()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var accounts = await _accountService.GetAccounts();
            var account = accounts.FirstOrDefault(a => a.AccountEmail == email);

            if (account == null)
            {
                TempData["ErrorMessage"] = "Account not found";
                return RedirectToAction("Index", "Home");
            }

            return View(account);
        }

        [HttpGet]
        public async Task<IActionResult> Edit()
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;
            var accounts = await _accountService.GetAccounts();
            var account = accounts.FirstOrDefault(a => a.AccountEmail == email);
            if (account == null)
            {
                return NotFound();
            }
            return View(account);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(SystemAccountDTO dto)
        {
            var email = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email)?.Value;

            // Bảo vệ tránh user sửa email hoặc tên tài khoản khác
            if (dto.AccountEmail != email)
            {
                return Unauthorized();
            }

            if (!ModelState.IsValid)
            {
                return View(dto);
            }

            try
            {
                await _accountService.Update(dto);
                TempData["SuccessMessage"] = "Profile updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                ModelState.AddModelError(string.Empty, "Error updating profile: " + ex.Message);
                return View(dto);
            }
        }
    }
}
