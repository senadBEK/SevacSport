using System.ComponentModel.DataAnnotations;

namespace WebShop.MVC.ViewModels
{
    public class LoginViewModel
    {
        [Display(Name = "Korisničko ime")]
        [Required(ErrorMessage = "Korisničko ime je obavezno.")]
        public string Username { get; set; } = string.Empty;

        [Display(Name = "Lozinka")]
        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        public string Password { get; set; } = string.Empty;

        public string? ReturnUrl { get; set; }
    }
}