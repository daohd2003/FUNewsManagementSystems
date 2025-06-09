using FUDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyMvcApp.Utilities;
using Newtonsoft.Json.Linq;
using Services;
using System.Net.Http;

namespace MyMvcApp.Controllers
{
    [ValidateJwtToken]
    public class AdminController : Controller
    {
        private readonly ISystemAccountService _accountService;
        private readonly INewsArticleService _newsArticleService;
        private readonly HttpClient _httpClient;

        public AdminController(ISystemAccountService accountService, INewsArticleService newsArticleService, IHttpClientFactory factory)
        {
            _accountService = accountService;
            _newsArticleService = newsArticleService;
            _httpClient = factory.CreateClient("ODataAPI");

        }

        public async Task<IActionResult> Index(
                string? searchTerm,
                string? sortField = "AccountName",
                string? sortDirection = "asc",
                int pageNumber = 1,
                int pageSize = 3)
        {
            int skip = (pageNumber - 1) * pageSize;

            // Xây dựng filter cho searchTerm nếu có
            string filter = "";
            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                // OData contains với AccountName hoặc AccountEmail
                filter = $"$filter=contains(AccountName,'{searchTerm}') or contains(AccountEmail,'{searchTerm}')";
            }

            // Xây dựng orderby cho sort
            string orderby = $"$orderby={sortField} {sortDirection}";

            // Kết hợp query OData
            string query = $"/odata/systemAccount?{filter}&{orderby}&$skip={skip}&$top={pageSize}&$count=true";

            var response = await _httpClient.GetAsync(query);
            if (!response.IsSuccessStatusCode)
                return View("Error");

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            var accounts = json["value"].ToObject<List<SystemAccountDTO>>();
            int totalCount = json["@odata.count"]?.Value<int>() ?? 0;

            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CurrentSortField = sortField;
            ViewBag.CurrentSortDirection = sortDirection;

            var model = new PagedListViewModel<SystemAccountDTO>
            {
                Items = accounts,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return View(model);
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
    }
}
