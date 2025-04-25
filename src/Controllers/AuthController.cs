using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using SystemsMonitoring.Models;
using SystemsMonitoring.Services;
using System.Security.Claims;

namespace SystemsMonitoring.Controllers
{
    public class AuthController : Controller
    {
        private readonly AuthService _authService;
        private readonly ILogger<AuthController> _logger;

        public AuthController(AuthService authService, ILogger<AuthController> logger)
        {
            _authService = authService;
            _logger = logger;
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = "/")
        {
            // Check if user is already authenticated
            if (User.Identity?.IsAuthenticated == true)
            {
                return Redirect(returnUrl);
            }

            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = "/")
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            // Validate credentials
            if (_authService.ValidateCredentials(model.Username, model.Password))
            {
                // Create claims principal
                var principal = _authService.CreateClaimsPrincipal(model.Username);

                // Sign in the user
                await HttpContext.SignInAsync(
                    CookieAuthenticationDefaults.AuthenticationScheme,
                    principal,
                    new AuthenticationProperties
                    {
                        IsPersistent = model.RememberMe,
                        ExpiresUtc = DateTimeOffset.UtcNow.AddDays(7) // Cookie expires in 7 days if "remember me" is selected
                    });

                _logger.LogInformation($"User {model.Username} logged in at {DateTime.UtcNow}");

                // Redirect to the return URL or homepage
                return LocalRedirect(returnUrl);
            }

            // If we got this far, something failed
            ModelState.AddModelError(string.Empty, "Invalid username or password");
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            _logger.LogInformation($"User {User.Identity?.Name} logged out at {DateTime.UtcNow}");

            // Sign out the user
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            // Redirect to home page
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}