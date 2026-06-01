using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using WebShop.BLL.Interfaces;
using WebShop.DAL;
using WebShop.DAL.Models;

namespace WebShop.BLL.Services
{
    public class UserService : IUserService
    {
        private readonly WebShopDbContext _context;

        public UserService(WebShopDbContext context)
        {
            _context = context;
        }

        public async Task<AppUser?> AuthenticateAsync(string username, string password)
        {
            var normalizedUsername = username.Trim().ToLower();

            var user = await _context.Users
                .FirstOrDefaultAsync(x => x.Username.ToLower() == normalizedUsername);

            if (user == null)
                return null;

            var hash = HashPassword(password);

            if (user.PasswordHash != hash)
                return null;

            return user;
        }

        public async Task EnsureSeedAdminAsync()
        {
            var usersToAdd = new List<AppUser>();

            var adminExists = await _context.Users.AnyAsync(x => x.Username == "admin");
            if (!adminExists)
            {
                usersToAdd.Add(new AppUser
                {
                    Username = "admin",
                    FullName = "Administrator",
                    Role = "Admin",
                    PasswordHash = HashPassword("admin123")
                });
            }

            var testExists = await _context.Users.AnyAsync(x => x.Username == "test");
            if (!testExists)
            {
                usersToAdd.Add(new AppUser
                {
                    Username = "test",
                    FullName = "Test Korisnik",
                    Role = "User",
                    PasswordHash = HashPassword("test123")
                });
            }

            if (usersToAdd.Any())
            {
                _context.Users.AddRange(usersToAdd);
                await _context.SaveChangesAsync();
            }
        }

        public async Task<List<AppUser>> GetAllAsync()
        {
            return await _context.Users
                .OrderBy(x => x.Username)
                .ToListAsync();
        }

        public async Task<AppUser?> GetByIdAsync(int id)
        {
            return await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<(bool Success, string Message)> RegisterAsync(string username, string password, string fullName)
        {
            var normalizedUsername = username.Trim().ToLower();

            var exists = await _context.Users.AnyAsync(x => x.Username.ToLower() == normalizedUsername);
            if (exists)
                return (false, "Korisničko ime već postoji.");

            var user = new AppUser
            {
                Username = username.Trim(),
                FullName = fullName.Trim(),
                Role = "User",
                PasswordHash = HashPassword(password)
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return (true, "Registracija je uspešna.");
        }

        public async Task<(bool Success, string Message)> DeleteAsync(int id, int currentUserId)
        {
            if (id == currentUserId)
                return (false, "Ne možete obrisati trenutno ulogovanog korisnika.");

            var user = await _context.Users.FirstOrDefaultAsync(x => x.Id == id);
            if (user == null)
                return (false, "Korisnik nije pronađen.");

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return (true, "Korisnik je obrisan.");
        }

        private static string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToHexString(hash);
        }
    }
}