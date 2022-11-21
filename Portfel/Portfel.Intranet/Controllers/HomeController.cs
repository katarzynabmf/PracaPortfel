﻿using Microsoft.AspNetCore.Mvc;
using Portfel.Data;
using Portfel.Intranet.Models;
using System.Diagnostics;

namespace Portfel.Intranet.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly PortfelContexts _context;

        public HomeController(ILogger<HomeController> logger, PortfelContexts context)
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
            ViewBag.TransakcjeIlosc = _context.Transakcja.Where(t => t.Aktywna == true).Count();
            ViewBag.TransakcjeIloscDzisiaj = _context.Transakcja.Where(t => t.Aktywna == true && t.DataUtworzenia.Date == DateTime.Today).Count();
            ViewBag.TransakcjeDodane = _context.Transakcja.Where(t => t.Aktywna).GroupBy(u => u.DataUtworzenia.Month).Select(u => new KeyValuePair<string, int>(u.Key.ToString(), u.Count())).ToList();  
                //.Select(t => new KeyValuePair<string, int>(t.DataUtworzenia.Month.ToString(), t.));
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

      

    }
}