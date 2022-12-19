using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Portfel.Data;
using Portfel.Data.Data;

namespace Portfel.Intranet.Controllers
{
    public class KontoGotowkoweController : Controller
    {
        private readonly PortfelContext _context;

        public KontoGotowkoweController(PortfelContext context)
        {
            _context = context;
        }

        // GET: Konto
        public async Task<IActionResult> Index()
        {
            var portfelContext = _context.KontaGotowkowe.Include(k => k.Portfel);
            return View(await portfelContext.ToListAsync());
        }

        // GET: Konto/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.KontaGotowkowe == null)
            {
                return NotFound();
            }

            var konto = await _context.KontaGotowkowe
                .Include(k => k.Portfel)
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
            ViewData["PortfelId"] = new SelectList(_context.Portfele, "Id", "Nazwa");
            return View();
        }

        // POST: Konto/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Nazwa,Waluta,Gotowka,UzytkownikId,Aktywna")] StworzKontoRequest stworzKonto)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new Konto()
                {
                    Nazwa = stworzKonto.Nazwa,
                    Gotowka = stworzKonto.Gotowka,
                    //Waluta = stworzKonto.Waluta, todo
                    UzytkownikId = stworzKonto.UzytkownikId,
                    Aktywna = true
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

            var konto = await _context.KontaGotowkowe.FindAsync(id);
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Waluta,Gotowka,UzytkownikId,Aktywna")] EdytujKontoRequest edytujKonto)
        {
            if (id != edytujKonto.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var konto = await _context.KontaGotowkowe.FindAsync(id);
                    if (konto == null)
                    {
                        return NotFound();
                    }
                    konto.Nazwa = edytujKonto.Nazwa;
                    konto.Gotowka = edytujKonto.Gotowka;
                    konto.Waluta = edytujKonto.Waluta;
                    konto.UzytkownikId = edytujKonto.UzytkownikId;
                    konto.Aktywna = edytujKonto.Aktywna;

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
            if (id == null || _context.KontaGotowkowe == null)
            {
                return NotFound();
            }

            var konto = await _context.KontaGotowkowe
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
            if (_context.KontaGotowkowe == null)
            {
                return Problem("Entity set 'PortfelContext.Konto'  is null.");
            }
            var konto = await _context.KontaGotowkowe.FindAsync(id);
            if (konto != null)
            {
                // _context.Konto.Remove(konto);
                konto.Aktywna = false;
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool KontoExists(int id)
        {
          return _context.KontaGotowkowe.Any(e => e.Id == id);
        }
    }
}
