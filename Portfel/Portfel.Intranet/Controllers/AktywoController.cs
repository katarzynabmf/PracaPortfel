﻿using System.Globalization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfel.Data;
using Portfel.Data.Data;
using Portfel.Data.Serwisy;

namespace Portfel.Intranet.Controllers
{
    public class AktywoController : Controller
    {
        private readonly PortfelContext _context;
        private readonly SymboleSerwis _symboleSerwis;
        private IEnumerable<Aktywo> _aktywa;

        public AktywoController(PortfelContext context, SymboleSerwis symboleSerwis)
        {
            _context = context;
            _symboleSerwis = symboleSerwis;
            
        }

        // GET: SymbolGieldowy
        public async Task<IActionResult> Index()
        {
            return View(await _context.Aktywa.ToListAsync());
        }

        // GET: SymbolGieldowy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Aktywa == null)
            {
                return NotFound();
            }

            var aktywo = await _context.Aktywa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aktywo == null)
            {
                return NotFound();
            }

            return View(aktywo);
        }

        // GET: SymbolGieldowy/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SymbolGieldowy/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,Symbol, CenaAktualna")] AktywoViewModel aktywo)
        {
            if (ModelState.IsValid)
            {
                var cena = Convert.ToDecimal(aktywo.CenaAktualna, Thread.CurrentThread.CurrentCulture);
                var noweAktywo = new Aktywo()
                {
                    Aktywna = true,
                    CenaAktualna = cena,
                    Id = aktywo.Id,
                    Nazwa = aktywo.Nazwa,
                    Symbol = aktywo.Symbol
                };
                _context.Add(noweAktywo);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aktywo);
        }

        // GET: SymbolGieldowy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Aktywa == null)
            {
                return NotFound();
            }

            var aktywo = await _context.Aktywa.FindAsync(id);
            if (aktywo == null)
            {
                return NotFound();
            }
            return View(new AktywoViewModel
            {
                Id = aktywo.Id,
                Nazwa = aktywo.Nazwa,
                Symbol = aktywo.Symbol,
                CenaAktualna = aktywo.CenaAktualna.ToString(Thread.CurrentThread.CurrentCulture),
                Aktywna = aktywo.Aktywna
            });
        }

        // POST: SymbolGieldowy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Symbol,CenaAktualna,Aktywna")] AktywoViewModel aktywoViewModel)
        {
            if (id != aktywoViewModel.Id) //todo
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(new Aktywo()
                    {
                        Nazwa = aktywoViewModel.Nazwa,
                        Aktywna = aktywoViewModel.Aktywna,
                        //    CenaAktualna = Convert.ToDecimal(aktywoViewModel.CenaAktualna, ci),
                        CenaAktualna = Convert.ToDecimal(aktywoViewModel.CenaAktualna, Thread.CurrentThread.CurrentCulture),
                        Id = aktywoViewModel.Id,
                        Symbol = aktywoViewModel.Symbol
                    });
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AktywoExists(aktywoViewModel.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(aktywoViewModel);
        }

        // GET: SymbolGieldowy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Aktywa == null)
            {
                return NotFound();
            }

            var aktywo = await _context.Aktywa
                .FirstOrDefaultAsync(m => m.Id == id);
            if (aktywo == null)
            {
                return NotFound();
            }

            return View(aktywo);
        }

        // POST: SymbolGieldowy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Aktywa == null)
            {
                return Problem("Entity set 'PortfelContext.SymbolGieldowy'  is null.");
            }
            var aktywo = await _context.Aktywa.FindAsync(id);
            if (aktywo != null)
            {
                aktywo.Aktywna = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AktywoExists(int id)
        {
            return _context.Aktywa.Any(e => e.Id == id);
        }

        public async Task<IActionResult> AktualizacjaCeny()
        {

            await _symboleSerwis.ZaktualizujCeny();
            return RedirectToAction(nameof(Index));
        }
        public IEnumerable<Aktywo> GetAktywa()
        {
            _aktywa = _context.Aktywa.ToList();
            return _aktywa;
        }

        public Aktywo GetAktywo(int id)
        {
            _aktywa = _context.Aktywa.ToList();
            return _aktywa.FirstOrDefault(p => p.Id == id);
        }

        [HttpPost]
        public Aktywo PostAktywo(string symbol, string nazwa, decimal cenaAktualna)
        {
            var aktywo = new Aktywo()
            {
                Nazwa = nazwa,
                Symbol = symbol,
                CenaAktualna = cenaAktualna,
                Aktywna = true
            };
            _context.Add(aktywo);
            _context.SaveChangesAsync();
            return aktywo;
        }

        [HttpPost]
        public Aktywo PatchAktywo(string symbol, decimal nowaCena)
        {
            var aktywoDoEdycji = _context.Aktywa.Single(aktywo => aktywo.Symbol == symbol);
            aktywoDoEdycji.CenaAktualna = nowaCena;

            var zaktualizowaneAktywo = _context.Update(aktywoDoEdycji);
            _context.SaveChangesAsync();
            return zaktualizowaneAktywo.Entity;
        }
    }
}
