using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.OData.Query;
using Microsoft.AspNetCore.OData.Routing.Controllers;
using Services;

namespace ODataAPI.Controllers
{
    [Route("odata/systemAccount")]
    public class AdminController : ODataController
    {
        private readonly ISystemAccountService _service;

        public AdminController(ISystemAccountService service)
        {
            _service = service;
        }

        [EnableQuery]
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            var articles = await _service.GetAccounts();
            return Ok(articles.AsQueryable());
        }
    }
}
