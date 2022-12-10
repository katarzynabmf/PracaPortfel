

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
                var portfel = _context.Portfele.Add(new Data.Data.Portfel()
                {
                    Nazwa = stworzPortfel.Nazwa,
                    UzytkownikId = uzytkownik.Id,
                    Waluta = "usd"
                });
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
                var kwota = Convert.ToDecimal(wplata.Kwota, Thread.CurrentThread.CurrentCulture);
                _portfelSerwis.WplacSrodkiNaKonto(kwota, id);

                //_portfelSerwis.WplacSrodkiNaKonto((decimal)wplata.Kwota, id);
                return RedirectToAction(nameof(MojePortfele));
            }
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email", uzytkownik.Id);
            return View("MojePortfele");
        }
        public IActionResult WyplacKwote()
        {
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email");
            return View("WyplacKwote");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult WyplacKwote([FromRoute] int id, [Bind("Kwota")] WyplataZPortfelaRequest wyplata)
        {
            var user = HttpContext.User.Identity;
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);
            if (ModelState.IsValid)
            {
                var kwota = Convert.ToDecimal(wyplata.Kwota, Thread.CurrentThread.CurrentCulture);
                _portfelSerwis.WplacSrodkiNaKonto(kwota, id);

                //_portfelSerwis.WyplacSrodkiZKonta((decimal)wyplata.Kwota, id);
                return RedirectToAction(nameof(MojePortfele));
            }
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email", uzytkownik.Id);
            return View("MojePortfele");
        }
        public IActionResult KupAktywo()
        {
            ViewData["AktywoId"] = new SelectList(_context.Aktywa, "Id", "Nazwa");
            return View("KupAktywo");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> KupAktywo(int id, [Bind("AktywoId, Cena, Ilosc, Komentarz")] KupAktywoRequest kupAktywo)
        {
            var user = HttpContext.User.Identity;
            var uzytkownik = await _context.Uzytkownik.FirstOrDefaultAsync(x => x.Email == user.Name);
            var portfel = await _context.Portfele.FindAsync(id);
            var aktywoSymbol = _context.Aktywa.FirstOrDefault(a => a.Id == kupAktywo.AktywoId).Symbol;
            var cena = Convert.ToDecimal(kupAktywo.Cena, Thread.CurrentThread.CurrentCulture);


            if (ModelState.IsValid)
            {
               
                _portfelSerwis.KupAktywo(aktywoSymbol, kupAktywo.Ilosc, cena, id, kupAktywo.Komentarz);
                return RedirectToAction(nameof(MojePortfele));
            }
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email", uzytkownik.Id);
            return View("MojePortfele");
        }
        public IActionResult SprzedajAktywo()
        {
            ViewData["AktywoId"] = new SelectList(_context.Aktywa, "Id", "Nazwa");
            return View("SprzedajAktywo");
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> SprzedajAktywo(int id, [Bind("AktywoId, Cena, Ilosc, Komentarz")] SprzedajAktywoRequest sprzedajAktywo)
        {
            var user = HttpContext.User.Identity;
            var uzytkownik = await _context.Uzytkownik.FirstOrDefaultAsync(x => x.Email == user.Name);
            var portfel = await _context.Portfele.FindAsync(id);
           
            var aktywoSymbol = _context.Aktywa.FirstOrDefault(a => a.Id == sprzedajAktywo.AktywoId).Symbol;

            var cena = Convert.ToDecimal(sprzedajAktywo.Cena, Thread.CurrentThread.CurrentCulture);
            if (ModelState.IsValid)
            {
                _portfelSerwis.SprzedajAktywo(aktywoSymbol, sprzedajAktywo.Ilosc,  cena, id, sprzedajAktywo.Komentarz);
                return RedirectToAction(nameof(MojePortfele));
            }
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email", uzytkownik.Id);
            return View("MojePortfele");
        }

        public async Task<IActionResult> SzczegolyPortfela(int id)
        {
            var user = HttpContext.User.Identity;
            if (user.Name == null)
            {
                return View("~/Views/Uzytkownik/StronaLogowania.cshtml");
            }
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);

            var portfel = _context.Portfele
                .Include(p => p.Pozycje)
                .ThenInclude(p => p.Aktywo)
                .FirstOrDefault(p => p.Id == id);
            var aktywa = _context.Aktywa.ToList();

            var szczegolyPozycji = portfel.Pozycje.Select(p => new PozycjaSzczegoly
            {
                Ilosc = p.Ilosc,
                SredniaCenaZakupu = p.SredniaCenaZakupu,
                NazwaAktywa = p.Aktywo.Nazwa,
                WartoscAktywa = p.Aktywo.CenaAktualna
            });
          

            return View("SzczegolyPortfela", new SzczegolyPortfela(id, portfel.Nazwa, szczegolyPozycji) { });
        }


        public async Task<IActionResult> WszytskieTransakcjeDlaPortfela(int id)
        {
            var user = HttpContext.User.Identity;
            if (user.Name == null)
            {
                return View("~/Views/Uzytkownik/StronaLogowania.cshtml");
            }
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);

            //var transakcje = _context.Transakcja.Include(t => t.IdKonta.Uzytkownik).
            //    Where(t => t.KontoId == id).ToList();
            var portfel = _context.Portfele.Include(p => p.Pozycje)
                .ThenInclude(p => p.Aktywo)
                .Include(p=>p.Transakcje)
                .Include(p => p.KontoGotowkowe)
                .ThenInclude(p => p.OperacjeGotowkowe)
                .FirstOrDefault(p => p.Id == id);
            
            return View("WszystkieTransakcje", new WszystkieTransakcjeDlaPortfela(id, portfel.Nazwa, portfel.Transakcje, portfel.KontoGotowkowe.Id, portfel.KontoGotowkowe.OperacjeGotowkowe) { });
        }

    }

    public class PozycjaSzczegoly
    {
        public string NazwaAktywa { get; set; }
        public decimal SredniaWartoscPozycji
        {
            get { return Ilosc * SredniaCenaZakupu; }
        }
        public decimal WartoscPozycji
        {
            get { return Ilosc * WartoscAktywa; }
        }
        public uint Ilosc { get; set; }
        public decimal SredniaCenaZakupu { get; set; }
        public  decimal WartoscAktywa { get; set; }

        public decimal ZyskStrata
        {
            get
            {
                return ((WartoscAktywa * 100) / SredniaCenaZakupu) - 100;
            }
        }
    }
}
