

using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Portfel.App.Models;
using Portfel.Data;
using Portfel.Data.Data;
using Portfel.Data.Serwisy;

namespace Portfel.App.Controllers
{
    public class PortfelController : Controller
    {
        private readonly PortfelContext _context;
        private readonly IPortfelSerwis _portfelSerwis;

        public PortfelController(PortfelContext context, IPortfelSerwis portfelSerwis)
        {
            _context = context; // tu inicjalizujemy baze danych
            _portfelSerwis = portfelSerwis;
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

            var portfele = _context.Portfele
                .Include(p => p.Uzytkownik)
                .Include(p => p.KontoGotowkowe)
                .Where(p => p.UzytkownikId == uzytkownik.Id && p.Aktywna == true).ToList();

            return View("MojePortfele", portfele);
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
                //_context.Portfele.Add(new Data.Data.Portfel(
                //    nazwa: stworzPortfel.Nazwa,
                //    uzytkownik: uzytkownik
                //));
                var portfel = _context.Portfele.Add(new Data.Data.Portfel()
                {
                    Nazwa = stworzPortfel.Nazwa,
                    UzytkownikId = uzytkownik.Id,
                    Waluta = "usd"
                });

                //var noweKonto = _context.Add(new KontoGotowkowe()
                //{
                //    PortfelId = portfel.Entity.Id

                //});


                _context.KontaGotowkowe.Add(new KontoGotowkowe(){Portfel = portfel.Entity});



                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MojePortfele));
            }
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email", uzytkownik.Id);
            return View("MojePortfele");
        }

        public IActionResult WplacKwote()
        {
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email");
            return View("WplacKwote");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult WplacKwote([FromRoute] int id, [Bind("Kwota")] WplataNaPortfelRequest wplata)
        {
            var user = HttpContext.User.Identity;
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);
            if (ModelState.IsValid)
            {
                _portfelSerwis.WplacSrodkiNaKonto((decimal)wplata.Kwota, id);
                return RedirectToAction(nameof(MojePortfele));
            }
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email", uzytkownik.Id);
            return View("MojePortfele");
        }


    }
}
