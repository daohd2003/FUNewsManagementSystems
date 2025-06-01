using FUDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Utilities;
using Services;

namespace MyMvcApp.Controllers
{
    [ValidateJwtToken]
    public class AdminController : Controller
    {
        private readonly ISystemAccountService _accountService;
        private readonly INewsArticleService _newsArticleService;

        public AdminController(ISystemAccountService accountService, INewsArticleService newsArticleService)
        {
            _accountService = accountService;
            _newsArticleService = newsArticleService;
        }

        public async Task<IActionResult> Index()
        {
            var accounts = await _accountService.GetAccounts();
            return View(accounts);
        }

        [HttpGet]
        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(SystemAccountDTO dto)
        {
            await _accountService.Create(dto);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Edit(short id)
        {
            var account = await _accountService.GetAccountById(id);
            return View(account);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(SystemAccountDTO dto)
        {
            await _accountService.Update(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(short id)
        {
            var account = await _accountService.GetAccountById(id);
            if (account == null) return NotFound();
            return View(account);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            await _accountService.Delete(id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult Report()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Report(DateTime startDate, DateTime endDate)
        {
            var articles = await _newsArticleService.GetNewsArticles();
            var filtered = articles
                .Where(a => a.CreatedDate >= startDate && a.CreatedDate <= endDate)
                .OrderByDescending(a => a.CreatedDate);
            return View("ReportResult", filtered);
        }
    }
}
