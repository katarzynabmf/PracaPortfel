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
        public async Task<IActionResult> Create([Bind("Id,Imie,Haslo,Email")] Uzytkownik uzytkownik, string powtorzHaslo)
        {
            var nowyUzytkownik = await _context.Uzytkownik.FirstOrDefaultAsync(u => u.Email == uzytkownik.Email);

            if (uzytkownik.Haslo == powtorzHaslo)
            {
                if (nowyUzytkownik == null)
                {
                    if (ModelState.IsValid)
                    {
                        await _context.AddAsync(uzytkownik);
                        await _context.SaveChangesAsync();
                        ViewBag.LoginMessage = "Konto zostało utworzone.";
                        return View("StronaLogowania");
                    }
                    return View("StronaRejestracji");
                }
                else
                {
                    ViewBag.RegisterErrorMessage = "Mail już jest zajęty. Zaloguj się lub użyj innego maila.";
                    return View("StronaRejestracji");
                }
            }
            ViewBag.PasswordErrorMessage = "Podane hasła różnią się od siebie.";
            return View("StronaRejestracji");

        }

        public IActionResult StronaLogowania(string? returnUrl = null)
        {
            if (returnUrl != null) 
                ViewData["ReturnUrl"] = returnUrl;
            return View("StronaLogowania");
        }

        [HttpPost]
        public async Task<IActionResult> Logowanie(string email, string haslo, string redirectUrl)
        {
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
                    return RedirectToAction("MojePortfele", "Portfel");
                }
            }
            ViewBag.LoginErrorMessage = "Mail lub hasło jest niepoprawne.";
            return View("StronaLogowania");
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

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return Redirect("/");
        }
    }
}
