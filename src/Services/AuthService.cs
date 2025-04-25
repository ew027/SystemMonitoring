using Microsoft.Extensions.Configuration;
using System.Security.Claims;

namespace SystemsMonitoring.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public bool ValidateCredentials(string username, string password)
        {
            // Get credentials from configuration
            var configUsername = _configuration["Authentication:AdminUser:Username"];
            var configPassword = _configuration["Authentication:AdminUser:Password"];

            // Validate credentials (simple string comparison)
            return username == configUsername && password == configPassword;
        }

        public ClaimsPrincipal CreateClaimsPrincipal(string username)
        {
            // Create the claims for this user
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Administrator"),
            };

            // Create the identity
            var identity = new ClaimsIdentity(claims, "SystemsMonitoring");

            // Create and return the principal
            return new ClaimsPrincipal(identity);
        }
    }
}