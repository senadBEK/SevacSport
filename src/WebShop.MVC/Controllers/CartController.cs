using Microsoft.AspNetCore.Mvc;
using WebShop.BLL.Interfaces;
using WebShop.MVC.Extensions;
using WebShop.MVC.ViewModels;
using Microsoft.AspNetCore.Authorization;

namespace WebShop.MVC.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly IProductService _productService;
        private const string CartKey = "CART";

        public CartController(IProductService productService)
        {
            _productService = productService;
        }

        private List<CartItemViewModel> GetCart()
        {
            return HttpContext.Session.GetObject<List<CartItemViewModel>>(CartKey) ?? new List<CartItemViewModel>();
        }

        private void SaveCart(List<CartItemViewModel> cart)
        {
            HttpContext.Session.SetObject(CartKey, cart);
        }

        public IActionResult Index()
        {
            var cart = GetCart();
            return View(cart);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Add(int productId)
        {
            var product = await _productService.GetByIdAsync(productId);
            if (product == null)
                return NotFound();

            var cart = GetCart();

            var existingItem = cart.FirstOrDefault(x => x.ProductId == productId);
            if (existingItem != null)
            {
                existingItem.Quantity++;
            }
            else
            {
                cart.Add(new CartItemViewModel
                {
                    ProductId = product.Id,
                    Name = product.Name,
                    Price = product.Price,
                    ImagePath = product.ImagePath,
                    Quantity = 1
                });
            }

            SaveCart(cart);
            return RedirectToAction("Index", "Product");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Increase(int productId)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                item.Quantity++;
            }

            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Decrease(int productId)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                item.Quantity--;

                if (item.Quantity <= 0)
                    cart.Remove(item);
            }

            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Remove(int productId)
        {
            var cart = GetCart();

            var item = cart.FirstOrDefault(x => x.ProductId == productId);
            if (item != null)
            {
                cart.Remove(item);
            }

            SaveCart(cart);
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Clear()
        {
            HttpContext.Session.Remove(CartKey);
            return RedirectToAction(nameof(Index));
        }
    }
}