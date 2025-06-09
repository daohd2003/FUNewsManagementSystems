using FUDTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using MyMvcApp.Utilities;
using BusinessObjects;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json.Linq;
using System.Drawing.Printing;
using System.Net.Http;

namespace MyMvcApp.Controllers
{
    [ValidateJwtToken]
    public class NewsArticleController : Controller
    {
        private readonly INewsArticleService _newsArticleService;
        private readonly ICategoryService _categoryService;
        private readonly ISystemAccountService _systemAccountService;
        private readonly HttpClient _httpClient;

        public NewsArticleController(INewsArticleService newsArticleService, ICategoryService categoryService, ISystemAccountService systemAccountService, IHttpClientFactory factory)
        {
            _newsArticleService = newsArticleService;
            _categoryService = categoryService;
            _systemAccountService = systemAccountService;
            _httpClient = factory.CreateClient("ODataAPI");
        }

        public async Task<IActionResult> Index(string searchTerm = "", string sortField = "CreatedDate", string sortDirection = "desc", int pageNumber = 1, int pageSize = 3)
        {
            int skip = (pageNumber - 1) * pageSize;

            // Filter theo từ khóa tìm kiếm
            string searchFilter = string.IsNullOrEmpty(searchTerm)
                ? ""
                : $"contains(NewsTitle,'{searchTerm}')";

            // Thêm filter theo NewsStatus nếu chưa đăng nhập
            string statusFilter = User.Identity.IsAuthenticated ? "" : "NewsStatus eq true";

            // Gộp các filter lại nếu cần
            string combinedFilter = "";
            if (!string.IsNullOrEmpty(searchFilter) && !string.IsNullOrEmpty(statusFilter))
            {
                combinedFilter = $"$filter=({searchFilter}) and ({statusFilter})&";
            }
            else if (!string.IsNullOrEmpty(searchFilter))
            {
                combinedFilter = $"$filter={searchFilter}&";
            }
            else if (!string.IsNullOrEmpty(statusFilter))
            {
                combinedFilter = $"$filter={statusFilter}&";
            }

            string orderByQuery = $"$orderby={sortField} {sortDirection}&";
            string pagingQuery = $"$skip={skip}&$top={pageSize}&$count=true";

            string query = $"/odata/newsArticles?{combinedFilter}{orderByQuery}{pagingQuery}";

            var response = await _httpClient.GetAsync(query);
            if (!response.IsSuccessStatusCode)
                return View("Error");

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);

            var articles = json["value"].ToObject<List<NewsArticleDTO>>();
            int totalCount = json["@odata.count"]?.Value<int>() ?? 0;

            var model = new PagedListViewModel<NewsArticleDTO>
            {
                Items = articles,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            ViewBag.CurrentSearch = searchTerm;
            ViewBag.CurrentSortField = sortField;
            ViewBag.CurrentSortDirection = sortDirection;

            return View(model);
        }

        public async Task<IActionResult> Details(string id)
        {
            var news = await _newsArticleService.GetNewsArticleById(id);
            if (news == null) return NotFound();
            return View(news);
        }

        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetCategories();
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");

            var users = await _systemAccountService.GetAccounts();
            ViewBag.CreatedById = new SelectList(users, "AccountId", "AccountName");
            ViewBag.UpdatedById = new SelectList(users, "AccountId", "AccountName");

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(NewsArticleDTO dto)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate dropdowns nếu ModelState không valid
                var categories = await _categoryService.GetCategories();
                ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName");

                var users = await _systemAccountService.GetAccounts();
                ViewBag.CreatedById = new SelectList(users, "AccountId", "AccountName");
                ViewBag.UpdatedById = new SelectList(users, "AccountId", "AccountName");
            }

            try
            {
                // Tự động tạo ID nếu chưa có
                if (string.IsNullOrEmpty(dto.NewsArticleId))
                {
                    dto.NewsArticleId = Guid.NewGuid().ToString();
                }

                // Thêm logging để kiểm tra
                Console.WriteLine($"Creating news article with ID: {dto.NewsArticleId}");

                dto.ModifiedDate = DateTime.Now;

                await _newsArticleService.Create(dto);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                // Log lỗi
                Console.WriteLine($"Error creating news article: {ex.Message}");

                TempData["ErrorMessage"] = "An error occurred while creating the news article";
                return RedirectToAction(nameof(Create));
            }
        }

        public async Task<IActionResult> Edit(string id)
        {
            var article = await _newsArticleService.GetNewsArticleById(id);
            if (article == null) return NotFound();

            var categories = await _categoryService.GetCategories();
            ViewBag.CategoryId = new SelectList(categories, "CategoryId", "CategoryName", article.CategoryId);

            var users = await _systemAccountService.GetAccounts();
            ViewBag.CreatedById = new SelectList(users, "AccountId", "AccountName", article.CreatedById);
            ViewBag.UpdatedById = new SelectList(users, "AccountId", "AccountName", article.UpdatedById);

            return View(article);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(string id, NewsArticleDTO dto)
        {
            if (id != dto.NewsArticleId) return BadRequest();
            if (!ModelState.IsValid) return View(dto);

            dto.ModifiedDate = DateTime.Now;

            await _newsArticleService.Update(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(string id)
        {
            var article = await _newsArticleService.GetNewsArticleById(id);
            if (article == null) return NotFound();
            return View(article);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ConfirmDelete(string id)
        {
            try
            {
                await _newsArticleService.Delete(id);

                TempData["SuccessMessage"] = "News article deleted successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting article: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while deleting the news article";
                return RedirectToAction(nameof(Delete), new { id });
            }
        }

        [HttpGet]
        public async Task<IActionResult> Detail(string id)
        {
            try
            {
                var newsArticle = await _newsArticleService.GetNewsArticleById(id);
                if (newsArticle == null)
                {
                    TempData["ErrorMessage"] = "News article not found";
                    return RedirectToAction(nameof(Index));
                }

                ViewBag.CategoryName = newsArticle.Category.CategoryName ?? "N/A";
                ViewBag.CreatedByName = newsArticle.CreatedBy.AccountName ?? "N/A";
                ViewBag.UpdatedByName = newsArticle.UpdatedBy.AccountName ?? "N/A";

                return View(newsArticle);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving news article details: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while retrieving news article details";
                return RedirectToAction(nameof(Index));
            }
        }

        public async Task<IActionResult> MyNews()
        {
            var myArticles = await _newsArticleService.GetNewsArticlesByAuthor();
            return View(myArticles);
        }
    }
}