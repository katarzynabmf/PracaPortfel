using Microsoft.AspNetCore.Mvc;
using Portfel.App.Models;
using System.Diagnostics;
using Portfel.Data;
using Portfel.Data.Data;

namespace Portfel.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly PortfelContexts _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController(PortfelContexts context)
        {
            _context = context; // tu inicjalizujemy baze danych
        }
        public IActionResult Index(int? id)
        {
            ViewBag.ModelSymboleGieldowe =
            (
                from symbol in _context.SymbolGieldowy
                select symbol
            ).ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Pomoc()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Symbole()
        {
            var symbole = from s in _context.SymbolGieldowy select s;
            ViewBag.ModelSymboleGieldowe =
            (
                from symbol in _context.SymbolGieldowy
                select symbol
            ).ToList();
            return View(symbole);
        }
    }
}