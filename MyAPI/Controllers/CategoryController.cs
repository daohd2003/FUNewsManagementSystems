using AutoMapper;
using BusinessObjects;
using FUDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Repositories;

namespace MyAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize(Roles = "Staff")]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepository;
        private readonly IMapper _mapper;

        public CategoryController(ICategoryRepository categoryRepository, IMapper mapper)
        {
            _categoryRepository = categoryRepository;
            _mapper = mapper;
        }

        // GET api/categories
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepository.GetAllCategory();  // List<Category>
            var categoryDTOs = _mapper.Map<IEnumerable<CategoryDTO>>(categories); // Map sang DTO
            return Ok(categoryDTOs);
        }

        // GET api/categories/{id}
        [HttpGet("{id}")]
        [AllowAnonymous]
        public async Task<IActionResult> GetById(short id)
        {
            var category = await _categoryRepository.GetCategoryById(id); // Trả về entity Category
            if (category == null) return NotFound();
            return Ok(category);
        }

        // POST api/categories
        [HttpPost]
        public async Task<IActionResult> Create(Category category)
        {
            await _categoryRepository.Add(category);
            return Content("Insert success!");
        }

        // PUT api/categories/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(short id, Category category)
        {
            if (id != category.CategoryId) return BadRequest();

            var existing = await _categoryRepository.GetCategoryById(id);
            if (existing == null) return NotFound();

            await _categoryRepository.Update(category);
            return Content("Update success!");
        }

        // DELETE api/categories/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(short id)
        {
            var category = await _categoryRepository.GetCategoryById(id);
            if (category == null) return NotFound();

            var hasNews = await _categoryRepository.HasNewsArticle(id);
            if (hasNews)
                return BadRequest("Cannot delete category because it has related news articles.");

            await _categoryRepository.Delete(id);
            return Content("Delete success!");
        }
    }
}
