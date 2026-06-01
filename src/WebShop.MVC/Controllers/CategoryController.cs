using Microsoft.AspNetCore.Mvc;
using WebShop.BLL.Interfaces;
using WebShop.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebShop.MVC.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryService _categoryService;

        public CategoryController(ICategoryService service)
        {
            _categoryService = service;
        }

        public async Task<IActionResult> Index()
        {
            var categories = await _categoryService.GetAllAsync();
            var vm = categories.Select(CategoryViewModel.FromEntity).ToList();
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            return View(CategoryViewModel.FromEntity(category));
        }

        [Authorize(Roles = "Admin")]
        public IActionResult Create() => View();

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel vm)
        {
            if (!ModelState.IsValid) return View(vm);

            await _categoryService.CreateAsync(vm.ToEntity());
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            return View(CategoryViewModel.FromEntity(category));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CategoryViewModel vm)
        {
            if (id != vm.Id) return BadRequest();
            if (!ModelState.IsValid) return View(vm);

            await _categoryService.UpdateAsync(vm.ToEntity());
            return RedirectToAction(nameof(Index));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            var category = await _categoryService.GetByIdAsync(id);
            if (category == null) return NotFound();

            return View(CategoryViewModel.FromEntity(category));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _categoryService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}
