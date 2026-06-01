using WebShop.DAL.Models;

namespace WebShop.BLL.Interfaces
{
    public interface IUserService
    {
        Task<AppUser?> AuthenticateAsync(string username, string password);
        Task EnsureSeedAdminAsync();

        Task<List<AppUser>> GetAllAsync();
        Task<AppUser?> GetByIdAsync(int id);
        Task<(bool Success, string Message)> RegisterAsync(string username, string password, string fullName);
        Task<(bool Success, string Message)> DeleteAsync(int id, int currentUserId);
    }
}