using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfel.Data;
using Portfel.Data.Data;

namespace Portfel.Intranet.Controllers
{
    public class SymbolGieldowyController : Controller
    {
        private readonly PortfelContexts _context;

        public SymbolGieldowyController(PortfelContexts context)
        {
            _context = context;
        }

        // GET: SymbolGieldowy
        public async Task<IActionResult> Index()
        {
              return View(await _context.SymbolGieldowy.ToListAsync());
        }

        // GET: SymbolGieldowy/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.SymbolGieldowy == null)
            {
                return NotFound();
            }

            var symbolGieldowy = await _context.SymbolGieldowy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (symbolGieldowy == null)
            {
                return NotFound();
            }

            return View(symbolGieldowy);
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
        public async Task<IActionResult> Create([Bind("Id,Nazwa")] SymbolGieldowy symbolGieldowy)
        {
            if (ModelState.IsValid)
            {
                _context.Add(symbolGieldowy);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(symbolGieldowy);
        }

        // GET: SymbolGieldowy/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.SymbolGieldowy == null)
            {
                return NotFound();
            }

            var symbolGieldowy = await _context.SymbolGieldowy.FindAsync(id);
            if (symbolGieldowy == null)
            {
                return NotFound();
            }
            return View(symbolGieldowy);
        }

        // POST: SymbolGieldowy/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa")] SymbolGieldowy symbolGieldowy)
        {
            if (id != symbolGieldowy.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(symbolGieldowy);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SymbolGieldowyExists(symbolGieldowy.Id))
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
            return View(symbolGieldowy);
        }

        // GET: SymbolGieldowy/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.SymbolGieldowy == null)
            {
                return NotFound();
            }

            var symbolGieldowy = await _context.SymbolGieldowy
                .FirstOrDefaultAsync(m => m.Id == id);
            if (symbolGieldowy == null)
            {
                return NotFound();
            }

            return View(symbolGieldowy);
        }

        // POST: SymbolGieldowy/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.SymbolGieldowy == null)
            {
                return Problem("Entity set 'PortfelContext.SymbolGieldowy'  is null.");
            }
            var symbolGieldowy = await _context.SymbolGieldowy.FindAsync(id);
            if (symbolGieldowy != null)
            {
                _context.SymbolGieldowy.Remove(symbolGieldowy);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SymbolGieldowyExists(int id)
        {
          return _context.SymbolGieldowy.Any(e => e.Id == id);
        }
    }
}
