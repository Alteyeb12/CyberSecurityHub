using CyberInsight.Data;
using CyberInsight.Models;
using BC = BCrypt.Net.BCrypt;
using Microsoft.EntityFrameworkCore;

namespace CyberInsight.Services
{
    public class AuthService : IAuthService
    {
        private readonly ApplicationDbContext _context;
        private readonly ILogger<AuthService> _logger;

        public AuthService(ApplicationDbContext context, ILogger<AuthService> logger)
        {
            _context = context;
            _logger = logger;
        }

        public async Task<User?> AuthenticateAsync(string email, string password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (user == null)
                {
                    _logger.LogWarning($"Login attempt failed for email: {email}");
                    return null;
                }

                if (!VerifyPassword(password, user.PasswordHash))
                {
                    _logger.LogWarning($"Invalid password attempt for: {email}");
                    return null;
                }

                user.LastLogin = DateTime.UtcNow;
                await _context.SaveChangesAsync();

                _logger.LogInformation($"User {email} logged in successfully");
                return user;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Authentication error: {ex.Message}");
                return null;
            }
        }

        public async Task<bool> RegisterAsync(string email, string firstName, string lastName, string password, string companyName)
        {
            try
            {
                var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
                if (existingUser != null)
                {
                    _logger.LogWarning($"Registration attempt with existing email: {email}");
                    return false;
                }

                var user = new User
                {
                    Email = email,
                    FirstName = firstName,
                    LastName = lastName,
                    PasswordHash = HashPassword(password),
                    CompanyName = companyName,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Create free subscription for new user
                var subscription = new Subscription
                {
                    UserId = user.Id,
                    Plan = SubscriptionPlan.Free,
                    Status = SubscriptionStatus.Active,
                    Price = 0
                };
                _context.Subscriptions.Add(subscription);
                await _context.SaveChangesAsync();

                _logger.LogInformation($"User {email} registered successfully");
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Registration error: {ex.Message}");
                return false;
            }
        }

        public async Task<User?> GetUserByEmailAsync(string email)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> ChangePasswordAsync(int userId, string currentPassword, string newPassword)
        {
            try
            {
                var user = await _context.Users.FindAsync(userId);
                if (user == null) return false;

                if (!VerifyPassword(currentPassword, user.PasswordHash))
                    return false;

                user.PasswordHash = HashPassword(newPassword);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                _logger.LogError($"Change password error: {ex.Message}");
                return false;
            }
        }

        public string HashPassword(string password)
        {
            return BC.HashPassword(password, workFactor: 12);
        }

        public bool VerifyPassword(string password, string hash)
        {
            try
            {
                return BC.Verify(password, hash);
            }
            catch
            {
                return false;
            }
        }
    }
}