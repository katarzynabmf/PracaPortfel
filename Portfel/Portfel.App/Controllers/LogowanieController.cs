using Microsoft.AspNetCore.Mvc;
using Portfel.App.Models;
using System.Diagnostics;


namespace Portfel.App.Controllers
{
    public class LogowanieController : Controller
    {
        private readonly ILogger<LogowanieController> _logger;

        public LogowanieController(ILogger<LogowanieController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
  
    }
}
