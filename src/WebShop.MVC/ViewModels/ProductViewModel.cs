using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using Humanizer;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.AspNetCore.Mvc.Rendering;
using WebShop.DAL.Models;

namespace WebShop.MVC.ViewModels
{
    public class ProductViewModel
    {
        public ProductViewModel()
        {
            Categories = new List<SelectListItem>();
        }

        public int Id { get; set; }

        [DisplayName("Šifra")]
        [Required(ErrorMessage = "Code is required")]
        public string Code { get; set; } = string.Empty;
        [DisplayName("Ime")]
        [Required(ErrorMessage = "Name is required")]
        public string Name { get; set; } = string.Empty;
        [DisplayName("Opis")]
        public string? Description { get; set; }

        [DisplayName("Cena")]
        [Range(0.01, 999999.99, ErrorMessage = "Cena mora biti veća od 0.")]
        public decimal Price { get; set; }

        [DisplayName("Brend")]
        public string? Brand { get; set; }

        [DisplayName("Sport")]
        public string? Sport { get; set; }

        [DisplayName("Veličina")]
        public string? Size { get; set; }

        [DisplayName("Kategorija")]
        public string? CategoryName { get; set; }
        
        public int? CategoryId { get; set; }

        public IFormFile? ImageFile { get; set; }
        [DisplayName("Slika proizvoda")]
        public string? ImagePath { get; set; }
        [DisplayName("Obriši sliku")]
        public bool DeleteImage { get; set; }

        public List<SelectListItem> Categories { get; set; }

        internal static ProductViewModel FromEntity(Product product)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description == null ? "Empty description" : product.Description,
                Price = product.Price,
                Brand = product.Brand,
                Sport = product.Sport,
                Size = product.Size,
                CategoryName = product.Category == null ? "No category" : product.Category.Name,
                CategoryId = product.CategoryId,
                ImagePath = product.ImagePath

            };
        }

        internal static ProductViewModel FromEntity(Product product, List<Category> categories = null)
        {
            return new ProductViewModel
            {
                Id = product.Id,
                Code = product.Code,
                Name = product.Name,
                Description = product.Description,
                Price = product.Price,
                Brand = product.Brand,
                Sport = product.Sport,
                Size = product.Size,
                CategoryName = product.Category == null ? "---" : product.Category.Name,
                CategoryId = product.CategoryId,
                ImagePath = product.ImagePath,
                Categories = (categories ?? new List<Category>()).Select(model => new SelectListItem
                {
                    Value = model.Id.ToString(),
                    Text = model.Code + "-" + model.Name,
                    Selected = model.Id == product.CategoryId
                }).ToList()
            };
        }

        internal Product ToEntity(string? imagePath = null, bool deleteImage = false)
        {
            return new Product
            {
                Id = this.Id,
                Code = this.Code.Trim(),
                Name = this.Name.Trim(),
                Description = this.Description?.Trim(),
                Price = this.Price,
                Brand = this.Brand?.Trim(),
                Sport = this.Sport?.Trim(),
                Size = this.Size?.Trim(),
                CategoryId = this.CategoryId,
                ImagePath = deleteImage ? null : (imagePath ?? this.ImagePath)
            };
        }
    }
}
