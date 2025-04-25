using System.Diagnostics;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using SystemMonitoring.Models;

namespace SystemMonitoring.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            // Get the system summary counts
            var summary = new
            {
                Healthy = 12,
                Warning = 3,
                Critical = 1
            };

            // Get the list of all systems with their basic metrics
            var systems = GetSystems();

            // Create the view model
            var viewModel = new
            {
                Summary = summary,
                Systems = systems
            };

            return View(viewModel);
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        // Helper method to generate dummy system data
        private List<SystemViewModel> GetSystems()
        {
            return new List<SystemViewModel>
            {
                new SystemViewModel
                {
                    Id = "db-server",
                    Name = "Database Server",
                    Icon = "fa-database",
                    Status = "warning",
                    Metrics = new Dictionary<string, string>
                    {
                        { "cpu", "42%" },
                        { "memory", "78%" },
                        { "disk", "89%" },
                        { "response", "120ms" }
                    }
                },
                new SystemViewModel
                {
                    Id = "web-server",
                    Name = "Web Server",
                    Icon = "fa-server",
                    Status = "healthy",
                    Metrics = new Dictionary<string, string>
                    {
                        { "cpu", "26%" },
                        { "memory", "45%" },
                        { "response", "87ms" },
                        { "requests", "348/s" }
                    }
                },
                new SystemViewModel
                {
                    Id = "app-server",
                    Name = "Application Server",
                    Icon = "fa-code",
                    Status = "healthy",
                    Metrics = new Dictionary<string, string>
                    {
                        { "cpu", "38%" },
                        { "memory", "56%" },
                        { "response", "95ms" },
                        { "users", "124" }
                    }
                },
                new SystemViewModel
                {
                    Id = "cache-server",
                    Name = "Cache Server",
                    Icon = "fa-bolt",
                    Status = "healthy",
                    Metrics = new Dictionary<string, string>
                    {
                        { "cpu", "15%" },
                        { "memory", "62%" },
                        { "response", "12ms" },
                        { "hit-rate", "94%" }
                    }
                },
                new SystemViewModel
                {
                    Id = "cdn",
                    Name = "Content Delivery Network",
                    Icon = "fa-globe",
                    Status = "critical",
                    Metrics = new Dictionary<string, string>
                    {
                        { "cpu", "92%" },
                        { "memory", "85%" },
                        { "response", "324ms" },
                        { "traffic", "256Mbps" }
                    }
                }
            };
        }
    }
}
