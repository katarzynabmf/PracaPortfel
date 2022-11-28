using Microsoft.AspNetCore.Mvc;
using Portfel.App.Models;
using System.Diagnostics;
using Portfel.Data;
using Microsoft.EntityFrameworkCore;

namespace Portfel.App.Controllers
{
    public class HomeController : Controller
    {
        private readonly PortfelContext _context;
        private readonly ILogger<HomeController> _logger;

        public HomeController (PortfelContext context)
        {
           
            _context = context; // tu inicjalizujemy baze danych
            //_context = new PortfelContext(new DbContextOptions<PortfelContext>());
        }
        public IActionResult Index()
        {
            //ViewBag.ModelSymboleGieldowe =
            //(
            //    from symbol in _context.SymbolGieldowy
            //    select symbol
            //).ToList();

            ViewBag.ModelAktualnosci =
            (
                from aktualnosc in _context.Aktualnosc
                orderby aktualnosc.DataDodania descending 
                select aktualnosc 
            ).ToList();

            return View();
            //ViewBag.ModelAktualnosci =
            //(
            //    from aktualnosc in _context.Aktualnosc
            //    select aktualnosc
            //).ToList();

            //return View();

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