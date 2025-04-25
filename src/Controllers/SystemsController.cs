using Microsoft.AspNetCore.Mvc;
using SystemsMonitoring.Models;
using System.Collections.Generic;
using SystemMonitoring.Models;
using Microsoft.AspNetCore.Authorization;

namespace SystemsMonitoring.Controllers
{
    [Authorize]
    public class SystemsController : Controller
    {
        private readonly ILogger<SystemsController> _logger;

        public SystemsController(ILogger<SystemsController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public IActionResult GetSystemDetails(string id)
        {
            // Log the request
            _logger.LogInformation($"Loading details for system: {id}");

            // Get the system details based on the ID
            var details = GetSystemDetailsById(id);

            if (details == null)
            {
                return NotFound();
            }

            // Return the partial view with the details
            return PartialView("_SystemDetails", details);
        }

        [HttpGet]
        public IActionResult GetSystemTab(string systemId, string tabId)
        {
            // Log the request
            _logger.LogInformation($"Loading tab {tabId} for system: {systemId}");

            // Get the system details
            var system = GetSystemDetailsById(systemId);

            if (system == null)
            {
                return NotFound();
            }

            // Determine which partial view to return based on the tab ID
            string partialViewName = tabId switch
            {
                "overview" => "_OverviewTab",
                "queries" => "_QueriesTab",
                "storage" => "_StorageTab",
                "traffic" => "_TrafficTab",
                "errors" => "_ErrorsTab",
                "performance" => "_PerformanceTab",
                _ => "_OverviewTab" // Default to overview
            };

            // Return the appropriate partial view with the system data
            return PartialView(partialViewName, system);
        }

        // Helper method to get system details by ID
        private SystemDetailsViewModel GetSystemDetailsById(string id)
        {
            // In a real application, this would fetch data from a database or service
            var allSystems = new Dictionary<string, SystemDetailsViewModel>
            {
                {
                    "db-server",
                    new SystemDetailsViewModel
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
                        },
                        ActiveConnections = new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                            {
                                { "id", "15234" },
                                { "user", "app_user" },
                                { "host", "10.0.1.45" },
                                { "database", "customers" },
                                { "command", "Query" },
                                { "time", "3s" },
                                { "state", "executing" }
                            },
                            new Dictionary<string, string>
                            {
                                { "id", "15235" },
                                { "user", "app_user" },
                                { "host", "10.0.1.46" },
                                { "database", "products" },
                                { "command", "Sleep" },
                                { "time", "120s" },
                                { "state", "idle" }
                            },
                            new Dictionary<string, string>
                            {
                                { "id", "15236" },
                                { "user", "admin" },
                                { "host", "10.0.1.1" },
                                { "database", "system" },
                                { "command", "Query" },
                                { "time", "15s" },
                                { "state", "executing" }
                            }
                        },
                        SlowQueries = new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                            {
                                { "query", "SELECT * FROM orders WHERE created_at > '2025-01-01'" },
                                { "executionTime", "3.45s" },
                                { "rowsExamined", "1,245,678" },
                                { "timestamp", "10:12:45" }
                            },
                            new Dictionary<string, string>
                            {
                                { "query", "SELECT c.name, sum(o.amount) FROM customers c JOIN orders o ON c.id = o.customer_id GROUP BY c.id" },
                                { "executionTime", "2.82s" },
                                { "rowsExamined", "856,342" },
                                { "timestamp", "09:34:21" }
                            },
                            new Dictionary<string, string>
                            {
                                { "query", "UPDATE products SET stock = stock - 1 WHERE id IN (SELECT product_id FROM order_items WHERE order_id = 45678)" },
                                { "executionTime", "1.95s" },
                                { "rowsExamined", "12,453" },
                                { "timestamp", "08:15:37" }
                            }
                        },
                        DatabaseSizes = new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                            {
                                { "database", "customers" },
                                { "size", "256 GB" },
                                { "tables", "24" },
                                { "indexes", "45" },
                                { "growth", "+6.5%" }
                            },
                            new Dictionary<string, string>
                            {
                                { "database", "products" },
                                { "size", "128 GB" },
                                { "tables", "12" },
                                { "indexes", "28" },
                                { "growth", "+2.1%" }
                            },
                            new Dictionary<string, string>
                            {
                                { "database", "orders" },
                                { "size", "512 GB" },
                                { "tables", "18" },
                                { "indexes", "36" },
                                { "growth", "+8.3%" }
                            },
                            new Dictionary<string, string>
                            {
                                { "database", "analytics" },
                                { "size", "750 GB" },
                                { "tables", "42" },
                                { "indexes", "78" },
                                { "growth", "+12.7%" }
                            }
                        }
                    }
                },
                {
                    "web-server",
                    new SystemDetailsViewModel
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
                        },
                        TopUrls = new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                            {
                                { "url", "/api/products" },
                                { "requests", "12,456" },
                                { "avgResponse", "65ms" },
                                { "errorRate", "0.02%" }
                            },
                            new Dictionary<string, string>
                            {
                                { "url", "/api/orders" },
                                { "requests", "8,973" },
                                { "avgResponse", "92ms" },
                                { "errorRate", "0.1%" }
                            },
                            new Dictionary<string, string>
                            {
                                { "url", "/api/users/auth" },
                                { "requests", "6,234" },
                                { "avgResponse", "45ms" },
                                { "errorRate", "0.05%" }
                            },
                            new Dictionary<string, string>
                            {
                                { "url", "/api/cart" },
                                { "requests", "4,532" },
                                { "avgResponse", "78ms" },
                                { "errorRate", "0.03%" }
                            }
                        },
                        LogEntries = new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                            {
                                { "time", "10:15:23" },
                                { "level", "INFO" },
                                { "message", "Server started on port 443" }
                            },
                            new Dictionary<string, string>
                            {
                                { "time", "10:16:45" },
                                { "level", "WARNING" },
                                { "message", "Slow response detected for /api/analytics/reports/monthly (1245ms)" }
                            },
                            new Dictionary<string, string>
                            {
                                { "time", "10:18:12" },
                                { "level", "ERROR" },
                                { "message", "404 Not Found: /api/products/discontinued" }
                            },
                            new Dictionary<string, string>
                            {
                                { "time", "10:20:34" },
                                { "level", "ERROR" },
                                { "message", "500 Internal Server Error: /api/inventory/sync (Database connection timeout)" }
                            },
                            new Dictionary<string, string>
                            {
                                { "time", "10:22:15" },
                                { "level", "INFO" },
                                { "message", "Cache cleared for product catalog" }
                            }
                        },
                        WorkerProcesses = new List<Dictionary<string, string>>
                        {
                            new Dictionary<string, string>
                            {
                                { "id", "worker-1" },
                                { "cpu", "32%" },
                                { "memory", "256 MB" },
                                { "connections", "124" },
                                { "uptime", "2d 14h" }
                            },
                            new Dictionary<string, string>
                            {
                                { "id", "worker-2" },
                                { "cpu", "28%" },
                                { "memory", "248 MB" },
                                { "connections", "118" },
                                { "uptime", "2d 14h" }
                            },
                            new Dictionary<string, string>
                            {
                                { "id", "worker-3" },
                                { "cpu", "35%" },
                                { "memory", "262 MB" },
                                { "connections", "136" },
                                { "uptime", "2d 14h" }
                            },
                            new Dictionary<string, string>
                            {
                                { "id", "worker-4" },
                                { "cpu", "24%" },
                                { "memory", "245 MB" },
                                { "connections", "108" },
                                { "uptime", "2d 14h" }
                            }
                        }
                    }
                }
                // Add more systems as needed
            };

            // Try to get the system with the given ID
            if (allSystems.TryGetValue(id, out var system))
            {
                return system;
            }

            return null;
        }
    }
}