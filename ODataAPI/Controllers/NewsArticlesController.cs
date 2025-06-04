using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services;

namespace ODataAPI.Controllers
{
    [Route("odata/NewsArticles")]
    public class NewsArticlesController : ODataController
    {
        private readonly INewsArticleService _service;

        public NewsArticlesController(INewsArticleService service)
        {
            _service = service;
        }

        [EnableQuery]
        [HttpGet("GetOData")]
        public async Task<IActionResult> Get()
        {
            var articles = await _service.GetNewsArticles();
            return Ok(articles.AsQueryable());
        }
    }
}
