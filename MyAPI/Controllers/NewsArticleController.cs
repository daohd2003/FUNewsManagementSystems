using AutoMapper;
using BusinessObjects;
using FUDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;
using System.Security.Claims;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff")]
    public class NewsArticleController : ControllerBase
    {
        private readonly INewsArticleRepository _newsRepo;
        private readonly IMapper _mapper;

        public NewsArticleController(INewsArticleRepository newsRepo, IMapper mapper)
        {
            _newsRepo = newsRepo;
            _mapper = mapper;
        }

        // Get all news (chỉ lấy news active hoặc theo yêu cầu)
        [HttpGet]
        [AllowAnonymous] // có thể để public xem tin bài
        public async Task<IActionResult> GetAll()
        {
            var news = await _newsRepo.GetAllNews();
            var dtos = _mapper.Map<IEnumerable<NewsArticleDTO>>(news);
            return Ok(dtos);
        }

        // Get by id
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(string id)
        {
            var news = await _newsRepo.GetNewsById(id);
            if (news == null) return NotFound();
            return Ok(news);
        }

        // Create news article
        [HttpPost]
        public async Task<IActionResult> Create(NewsArticleDTO dto)
        {
            var entity = _mapper.Map<NewsArticle>(dto);
            await _newsRepo.Add(entity);
            return Content("Insert success!");
        }

        // Update news article
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(string id, NewsArticleDTO dto)
        {
            if (id != dto.NewsArticleId) return BadRequest();
            var exist = await _newsRepo.GetNewsById(id);
            if (exist == null) return NotFound();

            var updated = _mapper.Map<NewsArticle>(dto);
            await _newsRepo.Update(updated);
            return Content("Update success!");
        }

        // Delete news article
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var news = await _newsRepo.GetNewsById(id);
            if (news == null) return NotFound();

            await _newsRepo.Delete(id);
            return Content("Delete success!");
        }

        [HttpGet("MyArticles")]
        public async Task<IActionResult> GetMyArticles()
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (!short.TryParse(userIdClaim, out short userId))
                return Unauthorized();

            var articles = await _newsRepo.GetByAuthor(userId);
            return Ok(articles);
        }
    }
}
