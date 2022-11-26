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
        public async Task<IActionResult> Create([Bind("KontoId,Date,RodzajTransakcjiId,Waluta,SymbolGieldowyId,Kwota,Ilosc,RodzajOplatyId,IloscRodzajuOplaty,Komentarz, Aktywna")] StworzTransakcjaRequest stworzTransakcja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Transakcja()

                    {
                        KontoId = stworzTransakcja.KontoId,
                        Date = stworzTransakcja.Date,
                        RodzajTransakcjiId = stworzTransakcja.RodzajTransakcjiId,
                        Waluta = stworzTransakcja.Waluta,
                        SymbolGieldowyId = stworzTransakcja.SymbolGieldowyId,
                        Kwota = stworzTransakcja.Kwota,
                        Ilosc = stworzTransakcja.Ilosc,
                        RodzajOplatyId = stworzTransakcja.RodzajOplatyId,
                    IloscRodzajuOplaty = stworzTransakcja.IloscRodzajuOplaty,
                    Komentarz = stworzTransakcja.Komentarz,
                        Aktywna = true
                    }
                );
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["KontoId"] = new SelectList(_context.Konto, "Id", "Nazwa", stworzTransakcja.KontoId);
            ViewData["RodzajOplatyId"] = new SelectList(_context.RodzajOplaty, "Id", "Nazwa", stworzTransakcja.RodzajOplatyId);
            ViewData["RodzajTransakcjiId"] = new SelectList(_context.RodzajTransakcji, "Id", "Nazwa", stworzTransakcja.RodzajTransakcjiId);
            ViewData["SymbolGieldowyId"] = new SelectList(_context.SymbolGieldowy, "Id", "Nazwa", stworzTransakcja.SymbolGieldowyId);
            return View(stworzTransakcja);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,KontoId,Date,RodzajTransakcjiId,Waluta,SymbolGieldowyId,Kwota,Ilosc,RodzajOplatyId,IloscRodzajuOplaty,Komentarz, Aktywna")] EdytujTransakcjaRequest edytujTransakcja)
        {
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
                    transakcja.Aktywna = edytujTransakcja.Aktywna;

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
                return RedirectToAction(nameof(Index));
            }
            ViewData["KontoId"] = new SelectList(_context.Konto, "Id", "Nazwa", edytujTransakcja.KontoId);
            ViewData["RodzajOplatyId"] = new SelectList(_context.RodzajOplaty, "Id", "Nazwa", edytujTransakcja.RodzajOplatyId);
            ViewData["RodzajTransakcjiId"] = new SelectList(_context.RodzajTransakcji, "Id", "Nazwa", edytujTransakcja.RodzajTransakcjiId);
            ViewData["SymbolGieldowyId"] = new SelectList(_context.SymbolGieldowy, "Id", "Nazwa", edytujTransakcja.SymbolGieldowyId);
            return View(edytujTransakcja);
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
                transakcja.Aktywna = false;
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
