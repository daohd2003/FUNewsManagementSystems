using FUDTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Services;
using MyMvcApp.Utilities;
using BusinessObjects;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace MyMvcApp.Controllers
{
    [ValidateJwtToken]
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService categoryService)
        {
            _categoryService = categoryService;
        }

        public async Task<IActionResult> Index()
        {
            try
            {
                var categories = await _categoryService.GetCategories();
                return View(categories);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving categories: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while loading categories";
                return View(new List<CategoryDTO>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetCategories();

            ViewBag.ParentCategories = new SelectList(categories, "CategoryId", "CategoryName");

            return View(new CategoryDTO { IsActive = true });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetCategories();
                ViewBag.ParentCategories = new SelectList(categories, "CategoryId", "CategoryName");
                return View(dto);
            }

            try
            {
                await _categoryService.Create(dto);
                TempData["SuccessMessage"] = "Category created successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating category: {ex.Message}");
                TempData["ErrorMessage"] = "Failed to create category";

                var categories = await _categoryService.GetCategories();
                ViewBag.ParentCategories = new SelectList(categories, "CategoryId", "CategoryName");
                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Edit(short id)
        {
            try
            {
                var category = await _categoryService.GetCategoryById(id);
                if (category == null)
                {
                    TempData["ErrorMessage"] = "Category not found";
                    return RedirectToAction(nameof(Index));
                }

                var categories = await _categoryService.GetCategories();
                var parentCandidates = categories.Where(c => c.CategoryId != id);
                ViewBag.ParentCategories = new SelectList(parentCandidates, "CategoryId", "CategoryName", category.ParentCategoryId);

                return View(category);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving category: {ex.Message}");
                TempData["ErrorMessage"] = "An error occurred while loading the category";
                return RedirectToAction(nameof(Index));
            }
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(CategoryDTO dto)
        {
            if (!ModelState.IsValid)
            {
                var categories = await _categoryService.GetCategories();
                var parentCandidates = categories.Where(c => c.CategoryId != dto.CategoryId);
                ViewBag.ParentCategories = new SelectList(parentCandidates, "CategoryId", "CategoryName", dto.ParentCategoryId);

                return View(dto);
            }

            try
            {
                await _categoryService.Update(dto);
                TempData["SuccessMessage"] = "Category updated successfully";
                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating category: {ex.Message}");
                TempData["ErrorMessage"] = "Failed to update category";

                var categories = await _categoryService.GetCategories();
                var parentCandidates = categories.Where(c => c.CategoryId != dto.CategoryId);
                ViewBag.ParentCategories = new SelectList(parentCandidates, "CategoryId", "CategoryName", dto.ParentCategoryId);

                return View(dto);
            }
        }

        [HttpGet]
        public async Task<IActionResult> Delete(short id)
        {
            var category = await _categoryService.GetCategoryById(id);
            if (category == null)
            {
                TempData["ErrorMessage"] = "Category not found";
                return RedirectToAction(nameof(Index));
            }
            return View(category);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(short id)
        {
            try
            {
                await _categoryService.Delete(id);
                TempData["SuccessMessage"] = "Category deleted successfully";
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting category: {ex.Message}");
                TempData["ErrorMessage"] = "Failed to delete category";
            }
            return RedirectToAction(nameof(Index));
        }
    }
}