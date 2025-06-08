using FUDTOs;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace HaDanhDao_SE17D10_A01_FE.Controllers
{
    public class ReportController : Controller
    {
        private readonly HttpClient _httpClient;

        public ReportController(IHttpClientFactory factory)
        {
            _httpClient = factory.CreateClient("ODataAPI");
        }

        [HttpGet]
        public IActionResult Report()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Report(DateTime startDate, DateTime endDate, int pageNumber = 1, int pageSize = 3)
        {
            int skip = (pageNumber - 1) * pageSize;

            string query = $"/odata/newsArticles" +
                           $"?$filter=CreatedDate ge {startDate.ToUniversalTime():yyyy-MM-ddTHH:mm:ssZ}" +
                           $" and CreatedDate lt {endDate.AddDays(1).ToUniversalTime():yyyy-MM-ddTHH:mm:ssZ}" +
                           $"&$orderby=CreatedDate desc" +
                           $"&$skip={skip}&$top={pageSize}&$count=true";

            var response = await _httpClient.GetAsync(query);
            if (!response.IsSuccessStatusCode) return View("Error");

            var content = await response.Content.ReadAsStringAsync();
            var json = JObject.Parse(content);
            var articles = json["value"].ToObject<List<NewsArticleDTO>>();
            int totalCount = json["@odata.count"]?.Value<int>() ?? 0;

            var model = new ReportViewModel
            {
                StartDate = startDate,
                EndDate = endDate,
                Articles = articles,
                PageNumber = pageNumber,
                PageSize = pageSize,
                TotalCount = totalCount
            };

            return View("ReportResult", model);
        }
    }
}
