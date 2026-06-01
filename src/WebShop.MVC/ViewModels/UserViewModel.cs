using WebShop.DAL.Models;

namespace WebShop.MVC.ViewModels
{
    public class UserViewModel
    {
        public int Id { get; set; }
        public string Username { get; set; } = string.Empty;
        public string FullName { get; set; } = string.Empty;
        public string Role { get; set; } = string.Empty;

        public static UserViewModel FromEntity(AppUser user)
        {
            return new UserViewModel
            {
                Id = user.Id,
                Username = user.Username,
                FullName = user.FullName,
                Role = user.Role
            };
        }
    }
}