using FUDTOs;
using HaDanhDao_SE17D10_A01_FE.DTOs.OData;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

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
        public async Task<IActionResult> Report(DateTime startDate, DateTime endDate)
        {
            string query = $"/odata/NewsArticles/GetOData?$filter=CreatedDate ge {startDate.ToUniversalTime():yyyy-MM-ddTHH:mm:ssZ} and CreatedDate lt {endDate.AddDays(1).ToUniversalTime():yyyy-MM-ddTHH:mm:ssZ}&$orderby=CreatedDate desc";

            var response = await _httpClient.GetAsync(query);
            if (!response.IsSuccessStatusCode) return View("Error");

            var content = await response.Content.ReadAsStringAsync();
            var result = JsonConvert.DeserializeObject<List<NewsArticleDTO>>(content);
            return View("ReportResult", result);
        }
    }
}
