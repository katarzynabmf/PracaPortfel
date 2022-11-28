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
       
        private readonly PortfelContext _context;
        public UzytkownikController(PortfelContext context)
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

            var wartoscKonta = _context.Konto.Where(k => k.UzytkownikId == uzytkownik.Id);

            //  var kontoUzytk = wartoscKonta.
              ViewBag.SumaKonto = _context.Konto.Where(k => k.UzytkownikId == uzytkownik.Id).Select(k => k.Gotowka).FirstOrDefault();
           // ViewBag.SumaKonto = _context.Konto.Where(k => k.UzytkownikId == uzytkownik.Id).Select(k => k.SumaNaKoncie).FirstOrDefault();
            // ViewBag.SumaKonto = 

            var portfelContext = _context.Konto
                .Include(k => k.Uzytkownik)
                .Include(k => k.Transakcje)
                .Where(k => k.UzytkownikId == uzytkownik.Id && k.Aktywna == true);
            return View("MojeKonta",await portfelContext.ToListAsync());

        }
        
        public async Task<IActionResult> SzczegolyKonta(int id)
        {
            var user = HttpContext.User.Identity;
            if (user.Name == null)
            {
                return View("StronaLogowania");
            }
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);

            //var transakcje = _context.Transakcja.Include(t => t.IdKonta.Uzytkownik).
            //    Where(t => t.KontoId == id).ToList();
            var konto = _context.Konto.Where(k => k.Id == id).FirstOrDefault();
            var nazwaKonta = konto.Nazwa;
            var transakcje =  _context.Transakcja.
                Include(t=>t.RodzajTransakcji).
                Include(t=>t.RodzajOplaty).
                Include(t=>t.SymbolGieldowy).
                Where(t => t.KontoId == id && t.Aktywna==true).ToList();
          
            return View("SzczegolyKonta", new SzczegolyKonta(id,nazwaKonta, transakcje){});
        }
        // GET: Konto/Edit/5
        public async Task<IActionResult> EdytujKonto(int id)
        {
            if (id == null || _context.Konto == null)
            {
                return NotFound();
            }

            var konto = await _context.Konto.FindAsync(id);
            if (konto == null)
            {
                return NotFound();
            }
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email", konto.UzytkownikId);
            return View(konto);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EdytujKonto(int id, [Bind("Id,Nazwa,Gotowka,UzytkownikId")] EdytujKontoRequest edytujKonto)
        {
            var user = HttpContext.User.Identity;
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);
            var portfelContext = _context.Konto.Include(k => k.Uzytkownik)
                .Where(k => k.UzytkownikId == uzytkownik.Id);


            if (user.Name == null)
            {
                return View("StronaLogowania");
            }
            if (id != edytujKonto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var konto = await _context.Konto.FindAsync(id);
                    if (konto == null)
                    {
                        return NotFound();
                    }
                    konto.Nazwa = edytujKonto.Nazwa;
                    konto.Gotowka = edytujKonto.Gotowka;
                    konto.Waluta = edytujKonto.Waluta;
                    konto.UzytkownikId = edytujKonto.UzytkownikId;
                    _context.Update(konto);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!KontoExists(edytujKonto.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("MojeKonta", await portfelContext.ToListAsync());
            }
  
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email", edytujKonto.UzytkownikId);
              return View("MojeKonta", await portfelContext.ToListAsync());
            //return RedirectToAction("MojeKonta", await portfelContext.ToListAsync());
        }

        // GET: Konto/Delete/5
        public async Task<IActionResult> UsunKonto(int id)
        {
            if (id == null || _context.Konto == null)
            {
                return NotFound();
            }

            var konto = await _context.Konto
                .Include(k => k.Uzytkownik)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (konto == null)
            {
                return NotFound();
            }

            return View(konto);
        }

        // POST: Konto/Delete/5
        [HttpPost, ActionName("UsunKonto")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var user = HttpContext.User.Identity;
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);
            var portfelContext = _context.Konto.Include(k => k.Uzytkownik)
                .Where(k => k.UzytkownikId == uzytkownik.Id);
            if (_context.Konto == null)
            {
                return Problem("Entity set 'PortfelContext.Konto'  is null.");
            }
            var konto = await _context.Konto.FindAsync(id);
            if (konto != null)
            {
                // _context.Konto.Remove(konto);
                konto.Aktywna = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(MojeKonta));

           // return View("MojeKonta", await portfelContext.ToListAsync());
        }


        // GET: Transakcja/Delete/5
        public async Task<IActionResult> UsunTransakcje(int id)
        {
            if (id == null || _context.Transakcja == null)
            {
                return NotFound();
            }

            var transakcja = await _context.Transakcja
                .Include(t => t.Konto)
                .Include(t => t.RodzajOplaty)
                .Include(t => t.RodzajTransakcji)
                .Include(t => t.SymbolGieldowy)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (transakcja == null)
            {
                return NotFound();
            }
            ViewData["KontoId"] = new SelectList(_context.Konto, "Id", "Nazwa", transakcja.KontoId);
            ViewData["RodzajOplatyId"] = new SelectList(_context.RodzajOplaty, "Id", "Nazwa", transakcja.RodzajOplatyId);
            ViewData["RodzajTransakcjiId"] = new SelectList(_context.RodzajTransakcji, "Id", "Nazwa", transakcja.RodzajTransakcjiId);
            ViewData["SymbolGieldowyId"] = new SelectList(_context.SymbolGieldowy, "Id", "Nazwa", transakcja.SymbolGieldowyId);
            return View(transakcja);
        }

        // POST: Transakcja/Delete/5
        [HttpPost, ActionName("UsunTransakcje")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> UsunT(int id)
        {
            if (_context.Transakcja == null)
            {
                return Problem("Entity set 'PortfelContext.Transakcja'  is null.");
            }
            var transakcja = await _context.Transakcja.FindAsync(id);
            if (transakcja != null)
            {
                transakcja.Aktywna = false;
            }

            var konto = transakcja.KontoId;
            await _context.SaveChangesAsync();
            return RedirectToAction("SzczegolyKonta", "Uzytkownik", new { id = konto });
        }


        private bool KontoExists(int id)
        {
            return _context.Konto.Any(e => e.Id == id);
        }
        // GET: Transakcja/Edit/5
        public async Task<IActionResult> EdytujTransakcje(int id)
        {

            if (id == null || _context.Transakcja == null)
            {
                return NotFound();
            }

            var transakcja = await _context.Transakcja.FindAsync(id);
            if (transakcja == null)
            {
                return NotFound();
            }
            ViewData["KontoId"] = new SelectList(_context.Konto, "Id", "Nazwa", transakcja.KontoId);
            ViewData["RodzajOplatyId"] = new SelectList(_context.RodzajOplaty, "Id", "Nazwa", transakcja.RodzajOplatyId);
            ViewData["RodzajTransakcjiId"] = new SelectList(_context.RodzajTransakcji, "Id", "Nazwa", transakcja.RodzajTransakcjiId);
            ViewData["SymbolGieldowyId"] = new SelectList(_context.SymbolGieldowy, "Id", "Nazwa", transakcja.SymbolGieldowyId);
            return View(transakcja);
        }

        // POST: Transakcja/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EdytujTransakcje(int id, [Bind("Id,KontoId,Date,RodzajTransakcjiId,Waluta,SymbolGieldowyId,Kwota,Ilosc,RodzajOplatyId,IloscRodzajuOplaty, Komentarz")] EdytujTransakcjaRequest edytujTransakcja)
        {
            var user = HttpContext.User.Identity;
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);
            var portfelContext = _context.Konto.Include(k => k.Uzytkownik)
                .Where(k => k.UzytkownikId == uzytkownik.Id);


            if (user.Name == null)
            {
                return View("StronaLogowania");
            }

            if (id != edytujTransakcja.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var transakcja = await _context.Transakcja.FindAsync(id);
                    if (transakcja == null)
                    {
                        return NotFound();
                    }

                    transakcja.KontoId = edytujTransakcja.KontoId;
                    transakcja.Date = edytujTransakcja.Date;
                    transakcja.RodzajTransakcjiId = edytujTransakcja.RodzajTransakcjiId;
                    transakcja.Waluta = edytujTransakcja.Waluta;
                    transakcja.SymbolGieldowyId = edytujTransakcja.SymbolGieldowyId;
                    transakcja.Kwota = edytujTransakcja.Kwota;
                    transakcja.Ilosc = edytujTransakcja.Ilosc;
                    transakcja.RodzajOplatyId = edytujTransakcja.RodzajOplatyId;
                    transakcja.IloscRodzajuOplaty = edytujTransakcja.IloscRodzajuOplaty;
                    transakcja.Komentarz = edytujTransakcja.Komentarz;

                    _context.Update(transakcja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransakcjaExists(edytujTransakcja.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("SzczegolyKonta", "Uzytkownik", new { id = edytujTransakcja.KontoId });
            }
            return RedirectToAction("SzczegolyKonta", "Uzytkownik", new { id = edytujTransakcja.KontoId });
        }
        private bool TransakcjaExists(int id)
        {
            return _context.Transakcja.Any(e => e.Id == id);
        }

        public IActionResult DodajKonto()
        {
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email");
            return View("DodajKonto");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DodajKonto([Bind("Nazwa,Gotowka")] StworzKontoRequest stworzKonto)
        {
            var user = HttpContext.User.Identity;
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);

            if (ModelState.IsValid)
            {
                _context.Add(new Konto()
                {
                    Nazwa = stworzKonto.Nazwa,
                    Gotowka = stworzKonto.Gotowka,
                    Waluta = "pln",
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
                    Where(t => t.KontoId == id && t.Aktywna==true).ToList();
                var nazwaKonta = konto.Nazwa;
                return View("SzczegolyKonta", new SzczegolyKonta(id,nazwaKonta, transakcje) { });

            }
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email", stworzTransakcja.KontoId);
            ViewData["KontoId"] = new SelectList(_context.Konto, "Id", "Nazwa", stworzTransakcja.KontoId);
            ViewData["RodzajOplatyId"] = new SelectList(_context.RodzajOplaty, "Id", "Nazwa", stworzTransakcja.RodzajOplatyId);
            ViewData["RodzajTransakcjiId"] = new SelectList(_context.RodzajTransakcji, "Id", "Nazwa", stworzTransakcja.RodzajTransakcjiId);
            ViewData["SymbolGieldowyId"] = new SelectList(_context.SymbolGieldowy, "Id", "Nazwa", stworzTransakcja.SymbolGieldowyId);
            //return View("SzczegolyKonta");
            //  return View(stworzTransakcja);
            return RedirectToAction("SzczegolyKonta", "Uzytkownik", new { id = stworzTransakcja.KontoId });
        }



        public async Task<IActionResult> MojeTransakcje()
        {
            var transakcje = from t in _context.Transakcja select t;
           
            var user = HttpContext.User.Identity;
            if (user.Name == null)
            {
                return View("StronaLogowania");
            }
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
