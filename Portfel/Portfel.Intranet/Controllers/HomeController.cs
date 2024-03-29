﻿using Microsoft.AspNetCore.Mvc;
using Portfel.Data;
using Portfel.Intranet.Models;
using System.Diagnostics;

namespace Portfel.Intranet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PortfelContext _context;

        public HomeController(ILogger<HomeController> logger, PortfelContext context)
        {
            _logger = logger;
            _context = context;

        }

        public IActionResult Index()
        {

            ViewBag.UzytkownicyIlosc = _context.Uzytkownik.Where(u => u.Aktywna == true).Count();
            ViewBag.UzytkownicyIloscDzisiaj = _context.Uzytkownik.Where(u => u.Aktywna == true && u.DataUtworzenia.Date == DateTime.Today).Count();
            ViewBag.UzytkownicyNieaktywni = _context.Uzytkownik.Where(u => u.Aktywna == false).Count();
            ViewBag.UzytkownicyAktywni = _context.Uzytkownik.Where(u => u.Aktywna == true).Count();
            ViewBag.TransakcjeNewIlosc = _context.TransakcjeNew.Where(t => t.Aktywna == true).Count();
            ViewBag.TransakcjeNewIloscDzisiaj = _context.TransakcjeNew.Where(t => t.Aktywna == true && t.DataTransakcji.Date == DateTime.Today).Count();
            ViewBag.TransakcjeDodane = _context.TransakcjeNew.Where(t => t.Aktywna).GroupBy(u => u.DataTransakcji.Month)
                .Select(u => new KeyValuePair<string, int>(u.Key.ToString(), u.Count())).ToList();
            ViewBag.AktualnoscOstatniaTytul = _context.Aktualnosc.Where(a => a.Pozycja == "Aktualnosc").TakeLast(1)
                .Select(a => new KeyValuePair<string, string>(a.Tytul, a.Tytul));
            ViewBag.Aktualnosci = _context.Aktualnosc.ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        public IActionResult AktualnosciIPosty()
        { 
            ViewBag.OstatniaAktualnosc = _context.Aktualnosc.Where(a => a.Pozycja == "Aktualnosc").TakeLast(1);
            ViewBag.OstatniPost = _context.Aktualnosc.Where(a => a.Pozycja == "Post").TakeLast(1);
   
            return View();
        }

    }
}