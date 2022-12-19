using Microsoft.AspNetCore.Mvc;
using Portfel.App.Models;
using System.Diagnostics;
using Portfel.Data;
using Microsoft.EntityFrameworkCore;
using Portfel.Data.Data;

namespace Portfel.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly PortfelContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController (PortfelContext context)
        {
            _context = context; // tu inicjalizujemy baze danych
        }
        public IActionResult Index()
        {
            IEnumerable<Aktualnosc> aktualnosci = _context.Aktualnosc.ToList();
            IEnumerable<Aktywo> aktywa = _context.Aktywa.ToList();
            ViewBag.ModelAktualnosciIIAktywa = new AktualnosciIAktywa(aktualnosci, aktywa);
            return View();
        }

        public async Task<ActionResult> Szczegoly(int id)
        {
            return View(await _context.Aktualnosc.Where(t => t.Id == id).FirstOrDefaultAsync());
        }

        public IActionResult Privacy()
        {
            return View();
        }
        public IActionResult Pomoc()
        {
            return View();
        }
        public IActionResult Kont()
        {
            return View();
        }
        public IActionResult Polityka()
        {
            return View();
        }
        public IActionResult Regulamin()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
        public IActionResult Aktualnosci()
        {
            var aktualnosci = from a in _context.Aktualnosc select a;
            ViewBag.ModelAktualnosci =
            (
                from a in _context.Aktualnosc
                select a
            ).ToList();
            return View(aktualnosci);
        }
    }
}