using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebShop.BLL.Interfaces;
using WebShop.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebShop.MVC.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICategoryService _categoryService;
        private readonly IWebHostEnvironment _env;

        public ProductController(
            IProductService service,
            ICategoryService categoryService,
            IWebHostEnvironment env)
        {
            _productService = service;
            _categoryService = categoryService;
            _env = env;
        }

        private async Task LoadCategories(ProductViewModel vm)
        {
            var categories = await _categoryService.GetAllAsync();

            vm.Categories = categories.Select(model => new SelectListItem
            {
                Value = model.Id.ToString(),
                Text = model.Code + "-" + model.Name,
                Selected = model.Id == vm.CategoryId
            }).ToList();
        }

        private async Task<string?> SaveImageAsync(IFormFile? imageFile)
        {
            if (imageFile == null || imageFile.Length == 0)
                return null;
            if (string.IsNullOrEmpty(_env.WebRootPath))
                throw new Exception("WebRootPath nije konfigurisan. Proveri da postoji wwwroot folder.");
            var allowedExtensions = new[] { ".jpg", ".jpeg", ".png", ".gif" };
            var extension = Path.GetExtension(imageFile.FileName).ToLowerInvariant();

            if (!allowedExtensions.Contains(extension))
                throw new Exception("Samo JPG, JPEG, PNG i GIF Slike su dozvoljene.");

            if (imageFile.Length > 5_000_000)
                throw new Exception("Slike moraju biti manje od 5 MB.");

            var uploadsFolder = Path.Combine(_env.WebRootPath, "uploads", "products");
            Directory.CreateDirectory(uploadsFolder);

            var fileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(uploadsFolder, fileName);

            await using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            return $"/uploads/products/{fileName}";
        }

        private void DeletePhysicalFile(string? imagePath)
        {
            if (string.IsNullOrWhiteSpace(imagePath))
                return;

            var relativePath = imagePath
                .TrimStart('/')
                .Replace("/", Path.DirectorySeparatorChar.ToString());

            var fullPath = Path.Combine(_env.WebRootPath, relativePath);

            if (System.IO.File.Exists(fullPath))
                System.IO.File.Delete(fullPath);
        }

        public async Task<IActionResult> Index()
        {
            var products = await _productService.GetAllAsync();
            var vm = products.Select(ProductViewModel.FromEntity).ToList();
            return View(vm);
        }

        public async Task<IActionResult> Details(int id)
        {
            var product = await _productService.GetByIdAsync(id);
            if (product == null) return NotFound();

            return View(ProductViewModel.FromEntity(product));
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create()
        {
            var categories = await _categoryService.GetAllAsync();

            var productViewModel = ProductViewModel.FromEntity(
                new WebShop.DAL.Models.Product() { Code = "", Name = "" },
                categories
            );

            return View(productViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel vm)
        {
            if (!ModelState.IsValid)
            {
                await LoadCategories(vm);
                return View(vm);
            }

            try
            {
                var imagePath = await SaveImageAsync(vm.ImageFile);

                var entity = vm.ToEntity(imagePath);
                await _productService.CreateAsync(entity);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await LoadCategories(vm);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productService.GetByIdAsync(id.Value);
            if (product == null) return NotFound();

            var categories = await _categoryService.GetAllAsync();
            var productViewModel = ProductViewModel.FromEntity(product, categories);

            return View(productViewModel);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, ProductViewModel vm)
        {
            if (id != vm.Id) return BadRequest();

            if (!ModelState.IsValid)
            {
                await LoadCategories(vm);
                return View(vm);
            }

            try
            {
                string? imagePath = vm.ImagePath;

                if (vm.DeleteImage && !string.IsNullOrWhiteSpace(vm.ImagePath))
                {
                    DeletePhysicalFile(vm.ImagePath);
                    imagePath = null;
                }

                if (vm.ImageFile != null && vm.ImageFile.Length > 0)
                {
                    if (!string.IsNullOrWhiteSpace(vm.ImagePath))
                    {
                        DeletePhysicalFile(vm.ImagePath);
                    }

                    imagePath = await SaveImageAsync(vm.ImageFile);
                }

                var entity = vm.ToEntity(imagePath, vm.DeleteImage);
                await _productService.UpdateAsync(entity);

                return RedirectToAction(nameof(Index));
            }
            catch (Exception ex)
            {
                await LoadCategories(vm);
                ModelState.AddModelError(string.Empty, ex.Message);
                return View(vm);
            }
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null) return NotFound();

            var product = await _productService.GetByIdAsync(id.Value);
            if (product == null) return NotFound();

            return View(ProductViewModel.FromEntity(product));
        }

        [Authorize(Roles = "Admin")]
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _productService.GetByIdAsync(id);

            if (product != null && !string.IsNullOrWhiteSpace(product.ImagePath))
            {
                DeletePhysicalFile(product.ImagePath);
            }

            await _productService.DeleteAsync(id);
            return RedirectToAction(nameof(Index));
        }
    }
}