using System.Security.Claims;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfel.App.Models;
using Portfel.Data;
using Portfel.Data.Data;
using Auth0.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;

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
                    _context.Add(uzytkownik);
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




        public IActionResult StronaLogowania()
        {
            return View("StronaLogowania");
        }

        [HttpPost]
        public async Task<IActionResult> Logowanie(string email, string haslo, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            // Normally Identity handles sign in, but you can do it directly
            if (ValidateLogin(email, haslo))
            {
                var claims = new List<Claim>
                {
                    new Claim("user", email),
                    new Claim("role", "Member")
                };

                await HttpContext.SignInAsync(
                    new ClaimsPrincipal(new ClaimsIdentity(claims, "Cookies", "user", "role")));

                if (Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                {
                    return Redirect("/");
                }
            }

            return View();
        }
        private bool ValidateLogin(string userName, string password)
        {
            // For this sample, all logins are successful.
            return true;
        }

        [HttpGet]
        [Authorize]
        public async Task<IActionResult> MojeTransakcje()
        {
            var transakcje = from t in _context.Transakcja select t;
           
            var user = HttpContext.User.Identity;
            var uzytkownik = _context.Uzytkownik.FirstOrDefault(x => x.Email == user.Name);

            //var tr = _context.Transakcja
            //    .Include(t => t.Konto.Uzytkownik)
            //    .Where(t => t.Konto.Uzytkownik.Email == uzytkownik.Email)
            //    .ToList();

            //var tr = _context.Transakcja
            //    .Include(t => t.Konto.Uzytkownik)
            //    .Where(t => t.Konto.Uzytkownik.Id == 3)
            //    .ToList(); //dziala

            var tr = _context.Transakcja
                .Include(t => t.Konto.Uzytkownik)
                .Where(t => t.Konto.Uzytkownik.Email == "aa")
                .ToList();//dziala
            //var tr = _context.Transakcja
            //    .Include(t => t.Konto.Uzytkownik)
            //    .Where(t => t.Konto.Uzytkownik.Email == uzytkownik.Email)
            //    .ToList();

            //var transakcja = _context.Transakcja
            //    .Include(t => t.Konto.UzytkownikId)

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
    }
}
