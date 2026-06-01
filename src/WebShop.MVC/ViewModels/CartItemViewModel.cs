namespace WebShop.MVC.ViewModels
{
    public class CartItemViewModel
    {
        public int ProductId { get; set; }
        public string Name { get; set; } = string.Empty;
        public decimal Price { get; set; }
        public string? ImagePath { get; set; }
        public int Quantity { get; set; }

        public decimal Total => Price * Quantity;
    }
}