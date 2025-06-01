using FUDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;

namespace MyMvcApp.Controllers
{
    [Authorize(Roles = "Staff")]
    public class TagController : Controller
    {
        private readonly ITagService _tagService;

        public TagController(ITagService tagService)
        {
            _tagService = tagService;
        }

        public async Task<IActionResult> Index()
        {
            var tags = await _tagService.GetTags();
            return View(tags);
        }

        public async Task<IActionResult> Details(int id)
        {
            var tag = await _tagService.GetTagById(id);
            if (tag == null) return NotFound();
            return View(tag);
        }

        public IActionResult Create() => View();

        [HttpPost]
        public async Task<IActionResult> Create(TagDTO dto)
        {
            if (!ModelState.IsValid) return View(dto);
            await _tagService.Create(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Edit(int id)
        {
            var tag = await _tagService.GetTagById(id);
            if (tag == null) return NotFound();
            return View(tag);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, TagDTO dto)
        {
            if (id != dto.TagId) return BadRequest();
            if (!ModelState.IsValid) return View(dto);

            await _tagService.Update(dto);
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> Delete(int id)
        {
            var tag = await _tagService.GetTagById(id);
            if (tag == null) return NotFound();
            return View(tag);
        }

        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> ConfirmDelete(int id)
        {
            await _tagService.Delete(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
