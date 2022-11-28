using System.Data.Entity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Portfel.Data;
using Portfel.Data.Data;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portfel.App.Controllers
{
    public class PortfelController : Controller
    {
        private readonly PortfelContext _context;
        public PortfelController(PortfelContext context)
        {
            _context = context; // tu inicjalizujemy baze danych
        }
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MojePortfele()
        {
            var user = HttpContext.User.Identity;
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);

            var wartoscKonta = _context.Portfele.Where(k => k.UzytkownikId == uzytkownik.Id);

            //  var kontoUzytk = wartoscKonta.
          //  ViewBag.SumaKonto = _context.Portfele.Where(k => k.UzytkownikId == uzytkownik.Id).Select(k => k.KontoGotowkowe.StanKonta).FirstOrDefault();

            // ViewBag.SumaKonto = 

            var porfele = _context.Portfele
                .Include(k => k.Uzytkownik)
                .Where(k => k.UzytkownikId == uzytkownik.Id && k.Aktywna == true).ToList();
             //return View("MojePortfele", await portfelContext.ToListAsync());
            return View("MojePortfele", porfele);
        }
        public IActionResult DodajPortfel()
        {
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email");
            return View("DodajPortfel");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajPortfel([Bind("Nazwa")] StworzPortfelRequest stworzPortfel)
        {
            var user = HttpContext.User.Identity;
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);

            if (ModelState.IsValid)
            {
                _context.Portfele.Add(new Data.Data.Portfel(
                    nazwa: stworzPortfel.Nazwa,
                    uzytkownik: uzytkownik
                ));
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MojePortfele));
            }
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email", uzytkownik.Id);
            return View("MojePortfele");
        }
    }
}
