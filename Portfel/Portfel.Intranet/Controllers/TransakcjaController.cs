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
    public class TransakcjaController : Controller
    {
        private readonly PortfelContext _context;

        public TransakcjaController(PortfelContext context)
        {
            _context = context;
        }

        // GET: Transakcja
        public async Task<IActionResult> Index()
        {
            var portfelContext = _context.Transakcja.Include(t => t.Konto).Include(t => t.RodzajOplaty).Include(t => t.RodzajTransakcji).Include(t => t.SymbolGieldowy);
            return View(await portfelContext.ToListAsync());
        }

        // GET: Transakcja/Details/5
        public async Task<IActionResult> Details(int? id)
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

            return View(transakcja);
        }

        // GET: Transakcja/Create
        public IActionResult Create()
        {
            ViewData["KontoId"] = new SelectList(_context.Konto, "Id", "Nazwa");
            ViewData["RodzajOplatyId"] = new SelectList(_context.RodzajOplaty, "Id", "Nazwa");
            ViewData["RodzajTransakcjiId"] = new SelectList(_context.RodzajTransakcji, "Id", "Nazwa");
            ViewData["SymbolGieldowyId"] = new SelectList(_context.SymbolGieldowy, "Id", "Nazwa");
            return View();
        }

        // POST: Transakcja/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,KontoId,Date,RodzajTransakcjiId,Waluta,SymbolGieldowyId,Kwota,Ilosc,RodzajOplatyId,Komentarz")] Transakcja transakcja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(transakcja);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KontoId"] = new SelectList(_context.Konto, "Id", "Nazwa", transakcja.KontoId);
            ViewData["RodzajOplatyId"] = new SelectList(_context.RodzajOplaty, "Id", "Nazwa", transakcja.RodzajOplatyId);
            ViewData["RodzajTransakcjiId"] = new SelectList(_context.RodzajTransakcji, "Id", "Nazwa", transakcja.RodzajTransakcjiId);
            ViewData["SymbolGieldowyId"] = new SelectList(_context.SymbolGieldowy, "Id", "Nazwa", transakcja.SymbolGieldowyId);
            return View(transakcja);
        }

        // GET: Transakcja/Edit/5
        public async Task<IActionResult> Edit(int? id)
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,KontoId,Date,RodzajTransakcjiId,Waluta,SymbolGieldowyId,Kwota,Ilosc,RodzajOplatyId,Komentarz")] Transakcja transakcja)
        {
            if (id != transakcja.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(transakcja);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TransakcjaExists(transakcja.Id))
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
            ViewData["KontoId"] = new SelectList(_context.Konto, "Id", "Nazwa", transakcja.KontoId);
            ViewData["RodzajOplatyId"] = new SelectList(_context.RodzajOplaty, "Id", "Nazwa", transakcja.RodzajOplatyId);
            ViewData["RodzajTransakcjiId"] = new SelectList(_context.RodzajTransakcji, "Id", "Nazwa", transakcja.RodzajTransakcjiId);
            ViewData["SymbolGieldowyId"] = new SelectList(_context.SymbolGieldowy, "Id", "Nazwa", transakcja.SymbolGieldowyId);
            return View(transakcja);
        }

        // GET: Transakcja/Delete/5
        public async Task<IActionResult> Delete(int? id)
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

            return View(transakcja);
        }

        // POST: Transakcja/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Transakcja == null)
            {
                return Problem("Entity set 'PortfelContext.Transakcja'  is null.");
            }
            var transakcja = await _context.Transakcja.FindAsync(id);
            if (transakcja != null)
            {
                _context.Transakcja.Remove(transakcja);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransakcjaExists(int id)
        {
          return _context.Transakcja.Any(e => e.Id == id);
        }
    }
}
