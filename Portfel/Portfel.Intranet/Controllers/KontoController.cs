using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Portfel.Data;
using Portfel.Data.Data;

namespace Portfel.Intranet.Controllers
{
    public class KontoController : Controller
    {
        private readonly PortfelContexts _context;

        public KontoController(PortfelContexts context)
        {
            _context = context;
        }

        // GET: Konto
        public async Task<IActionResult> Index()
        {
            var portfelContext = _context.Konto.Include(k => k.Uzytkownik);
            return View(await portfelContext.ToListAsync());
        }

        // GET: Konto/Details/5
        public async Task<IActionResult> Details(int? id)
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

        // GET: Konto/Create
        public IActionResult Create()
        {
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email");
            return View();
        }

        // POST: Konto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nazwa,Waluta,Gotowka,UzytkownikId")] StworzKontoRequest stworzKonto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Konto()
                {
                    Nazwa = stworzKonto.Nazwa,
                    Gotowka = stworzKonto.Gotowka,
                    Waluta = stworzKonto.Waluta,
                    UzytkownikId = stworzKonto.UzytkownikId
                });
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email", stworzKonto.UzytkownikId);
            return View(stworzKonto);
        }

        // GET: Konto/Edit/5
        public async Task<IActionResult> Edit(int? id)
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

        // POST: Konto/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Waluta,Gotowka,UzytkownikId")] EdytujKontoRequest edytujKonto)
        {
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
                return RedirectToAction(nameof(Index));
            }
            ViewData["UzytkownikId"] = new SelectList(_context.Uzytkownik, "Id", "Email", edytujKonto.UzytkownikId);
            return View(edytujKonto);
        }

        // GET: Konto/Delete/5
        public async Task<IActionResult> Delete(int? id)
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
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Konto == null)
            {
                return Problem("Entity set 'PortfelContext.Konto'  is null.");
            }
            var konto = await _context.Konto.FindAsync(id);
            if (konto != null)
            {
                _context.Konto.Remove(konto);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KontoExists(int id)
        {
          return _context.Konto.Any(e => e.Id == id);
        }
    }
}
