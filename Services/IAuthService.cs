using CyberInsight.Models;

namespace CyberInsight.Services
{
    public interface IAuthService
    {
        Task<User?> AuthenticateAsync(string email, string password);
        Task<bool> RegisterAsync(string email, string firstName, string lastName, string password, string companyName);
        Task<User?> GetUserByEmailAsync(string email);
        Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword);
        string HashPassword(string password);
        bool VerifyPassword(string password, string hash);
    }
}