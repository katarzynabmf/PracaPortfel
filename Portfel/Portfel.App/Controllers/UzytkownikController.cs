using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfel.App.Models;
using Portfel.Data;
using Portfel.Data.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Portfel.App.Controllers
{


    public class UzytkownikController : Controller
    {
       
        private readonly PortfelContexts _context;
        public UzytkownikController(PortfelContexts context)
        {
            _context = context; // tu inicjalizujemy baze danych
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult RejestracjaPomyslna()
        {
            return View("RejestracjaPomyslna");
        }
        public IActionResult StronaRejestracji()
        {
            return View("StronaRejestracji");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Imie,Haslo,Email")] Uzytkownik uzytkownik)
        {
            var nowyUzytkownik = await _context.Uzytkownik.FirstOrDefaultAsync(u => u.Email == uzytkownik.Email);
            if (nowyUzytkownik == null)
            {
                if (ModelState.IsValid)
                {
                    await _context.AddAsync(uzytkownik);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(RejestracjaPomyslna));
                }
                return View("StronaLogowania");
            }
            else
            {
                return View("RejestracjaNiepomyslna");
            }
        }

        public IActionResult StronaLogowania(string? returnUrl = null)
        {
            if (returnUrl != null) 
                ViewData["ReturnUrl"] = returnUrl;
            return View("StronaLogowania");
        }

        [HttpPost]
        public async Task<IActionResult> Logowanie(string email, string haslo, string redirectUrl)
        //public async Task<IActionResult> Logowanie(DaneLogowania daneLogowania)
        {
           
          //  redirectUrl = "/Uzytkownik/MojeTransakcje";
            // Normally Identity handles sign in, but you can do it directly
            if (await ValidateLogin(email, haslo))
            {
                var claims = new List<Claim>
                {
                    new Claim("user", email),
                    new Claim("role", "Member")
                };

                await HttpContext.SignInAsync(
                    new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));

                if (Url.IsLocalUrl(redirectUrl))
                {
                    return Redirect(redirectUrl);
                }
                else
                {
                    return Redirect("MojeKonta");
                }
            }
            
            return View("RejestracjaNiepomyslna");
        }
        private async Task<bool> ValidateLogin(string email, string haslo)
        {
            var uzytkownik = await _context.Uzytkownik.FirstOrDefaultAsync(x => x.Email == email);
            var ctx = HttpContext.User;
            if (uzytkownik == null)
            {
                return false;
            }

            return uzytkownik.Haslo == haslo;
        }
        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MojeKonta()
        {
            var user = HttpContext.User.Identity;
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);

            //var konto = _context.IdKonta
            //    .Include(k => k.Uzytkownik)
            //    .Where(k => k.UzytkownikId == uzytkownik.Id)
            //    .ToList();//dziala

            ////var mK = new MojeKonta
            ////{
            ////    Konta = konto,
            ////};

            //return View("MojeKonta", konto);


            var portfelContext = _context.Konto.Include(k => k.Uzytkownik)
                .Where(k => k.UzytkownikId == uzytkownik.Id);
            return View("MojeKonta",await portfelContext.ToListAsync());

        }

        public async Task<IActionResult> SzczegolyKonta(int id)
        {
            var user = HttpContext.User.Identity;
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);

            //var transakcje = _context.Transakcja.Include(t => t.IdKonta.Uzytkownik).
            //    Where(t => t.KontoId == id).ToList();
            var konto = _context.Konto.Where(k => k.Id == id).FirstOrDefault();
            var nazwaKonta = konto.Nazwa;
            var transakcje =  _context.Transakcja.
                Include(t=>t.RodzajTransakcji).
                Include(t=>t.RodzajOplaty).
                Include(t=>t.SymbolGieldowy).
                Where(t => t.KontoId == id).ToList();
            return View("SzczegolyKonta", new SzczegolyKonta(id,nazwaKonta, transakcje){});
        }
        public IActionResult DodajKonto()
        {
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email");
            return View("DodajKonto");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajKonto([Bind("Nazwa,Waluta,Gotowka")] StworzKontoRequest stworzKonto)
        {
            var user = HttpContext.User.Identity;
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);

            if (ModelState.IsValid)
            {
                _context.Add(new Konto()
                {
                    Nazwa = stworzKonto.Nazwa,
                    Gotowka = stworzKonto.Gotowka,
                    Waluta = stworzKonto.Waluta,
                    UzytkownikId = uzytkownik.Id
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(MojeKonta));
            }
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email", stworzKonto.UzytkownikId);
            return View("MojeKonta");
        }

        public IActionResult DodajTransakcje()
        {
            ViewData["RodzajOplatyId"] = new SelectList(_context.RodzajOplaty, "Id", "Nazwa");
            ViewData["RodzajTransakcjiId"] = new SelectList(_context.RodzajTransakcji, "Id", "Nazwa");
            ViewData["SymbolGieldowyId"] = new SelectList(_context.SymbolGieldowy, "Id", "Nazwa");
            return View("DodajTransakcje");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajTransakcje(int id, [Bind("KontoId,RodzajTransakcjiId, Waluta,SymbolGieldowyId, Kwota, Ilosc,RodzajOplatyId, Komentarz")] StworzTransakcjaRequest stworzTransakcja)
        {
            var user = HttpContext.User.Identity;
            var uzytkownik = await _context.Uzytkownik.FirstOrDefaultAsync(x => x.Email == user.Name);
            var konto = await _context.Konto.FindAsync(id);
            if (ModelState.IsValid)
            {
                await _context.AddAsync(new Transakcja()
                    {
                        KontoId = id,
                        Date = DateTime.Now,
                        RodzajTransakcjiId = stworzTransakcja.RodzajTransakcjiId,
                        Waluta = stworzTransakcja.Waluta,
                        SymbolGieldowyId = stworzTransakcja.SymbolGieldowyId,
                        Kwota = stworzTransakcja.Kwota,
                        Ilosc = stworzTransakcja.Ilosc,
                        RodzajOplatyId = stworzTransakcja.RodzajOplatyId,
                        Komentarz = stworzTransakcja.Komentarz
                    }
                );
                await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(MojeTransakcje));
                //return RedirectToAction(nameof(SzczegolyKonta), id);
                var transakcje = _context.Transakcja.
                    Include(t => t.RodzajTransakcji).
                    Include(t => t.RodzajOplaty).
                    Include(t => t.SymbolGieldowy).
                    Where(t => t.KontoId == id).ToList();
                var nazwaKonta = konto.Nazwa;
                return View("SzczegolyKonta", new SzczegolyKonta(id,nazwaKonta, transakcje) { });

            }
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email", stworzTransakcja.KontoId);
            ViewData["KontoId"] = new SelectList(_context.Konto, "Id", "Nazwa", stworzTransakcja.KontoId);
            ViewData["RodzajOplatyId"] = new SelectList(_context.RodzajOplaty, "Id", "Nazwa", stworzTransakcja.RodzajOplatyId);
            ViewData["RodzajTransakcjiId"] = new SelectList(_context.RodzajTransakcji, "Id", "Nazwa", stworzTransakcja.RodzajTransakcjiId);
            ViewData["SymbolGieldowyId"] = new SelectList(_context.SymbolGieldowy, "Id", "Nazwa", stworzTransakcja.SymbolGieldowyId);
            //return View("SzczegolyKonta");
            return View(stworzTransakcja);
        }



        public async Task<IActionResult> MojeTransakcje()
        {
            var transakcje = from t in _context.Transakcja select t;
           
            var user = HttpContext.User.Identity;
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);

            //var tr = _context.Transakcja
            //    .Include(t => t.IdKonta.Uzytkownik)
            //    .Where(t => t.IdKonta.Uzytkownik.Email == uzytkownik.Email)
            //    .ToList();

            //var tr = _context.Transakcja
            //    .Include(t => t.IdKonta.Uzytkownik)
            //    .Where(t => t.IdKonta.Uzytkownik.Id == 3)
            //    .ToList(); //dziala

            var tr = _context.Transakcja
                //.Include(t => t.IdKonta.Uzytkownik)
                .Where(t => t.Konto.Uzytkownik.Id == uzytkownik.Id)
                .ToList();//dziala
            //var tr = _context.Transakcja
            //    .Include(t => t.IdKonta.Uzytkownik)
            //    .Where(t => t.IdKonta.Uzytkownik.Email == uzytkownik.Email)
            //    .ToList();

            //var transakcja = _context.Transakcja
            //    .Include(t => t.IdKonta.UzytkownikId)

            //    .Where(t => t.KontoId == uzytkownik.Id)
            //    .ToList();
            var mT = new MojeTransakcje
            {
                Transakcje = tr,
            };

            var transa = _context.Transakcja.ToList();

            //return View("MojeTransakcje", mT);
            return View("MojeTransakcje", tr);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
