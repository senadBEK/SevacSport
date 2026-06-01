using System.ComponentModel.DataAnnotations;

namespace WebShop.MVC.ViewModels
{
    public class RegisterViewModel
    {
        [Display(Name = "Korisničko ime")]
        [Required(ErrorMessage = "Korisničko ime je obavezno.")]
        public string Username { get; set; } = string.Empty;

        [Display(Name = "Ime i prezime")]
        [Required(ErrorMessage = "Ime i prezime je obavezno.")]
        public string FullName { get; set; } = string.Empty;

        [Display(Name = "Lozinka")]
        [Required(ErrorMessage = "Lozinka je obavezna.")]
        [DataType(DataType.Password)]
        [MinLength(4, ErrorMessage = "Lozinka mora imati najmanje 4 karaktera.")]
        public string Password { get; set; } = string.Empty;

        [Display(Name = "Potvrda lozinke")]
        [Required(ErrorMessage = "Potvrda lozinke je obavezna.")]
        [DataType(DataType.Password)]
        [Compare("Password", ErrorMessage = "Lozinke se ne poklapaju.")]
        public string ConfirmPassword { get; set; } = string.Empty;
    }
}