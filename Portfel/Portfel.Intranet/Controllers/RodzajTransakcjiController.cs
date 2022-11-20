using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfel.Data;
using Portfel.Data.Data;

namespace Portfel.Intranet.Controllers
{
    public class RodzajTransakcjiController : Controller
    {
        private readonly PortfelContexts _context;

        public RodzajTransakcjiController(PortfelContexts context)
        {
            _context = context;
        }

        // GET: RodzajTransakcji
        public async Task<IActionResult> Index()
        {
              return View(await _context.RodzajTransakcji.ToListAsync());
        }

        // GET: RodzajTransakcji/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RodzajTransakcji == null)
            {
                return NotFound();
            }

            var rodzajTransakcji = await _context.RodzajTransakcji
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rodzajTransakcji == null)
            {
                return NotFound();
            }

            return View(rodzajTransakcji);
        }

        // GET: RodzajTransakcji/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RodzajTransakcji/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa, Aktywna")] RodzajTransakcji rodzajTransakcji)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rodzajTransakcji);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rodzajTransakcji);
        }

        // GET: RodzajTransakcji/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RodzajTransakcji == null)
            {
                return NotFound();
            }

            var rodzajTransakcji = await _context.RodzajTransakcji.FindAsync(id);
            if (rodzajTransakcji == null)
            {
                return NotFound();
            }
            return View(rodzajTransakcji);
        }

        // POST: RodzajTransakcji/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa, Aktywna")] RodzajTransakcji rodzajTransakcji)
        {
            if (id != rodzajTransakcji.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rodzajTransakcji);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RodzajTransakcjiExists(rodzajTransakcji.Id))
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
            return View(rodzajTransakcji);
        }

        // GET: RodzajTransakcji/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RodzajTransakcji == null)
            {
                return NotFound();
            }

            var rodzajTransakcji = await _context.RodzajTransakcji
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rodzajTransakcji == null)
            {
                return NotFound();
            }

            return View(rodzajTransakcji);
        }

        // POST: RodzajTransakcji/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RodzajTransakcji == null)
            {
                return Problem("Entity set 'PortfelContext.RodzajTransakcji'  is null.");
            }
            var rodzajTransakcji = await _context.RodzajTransakcji.FindAsync(id);
            if (rodzajTransakcji != null)
            {
                // _context.RodzajTransakcji.Remove(rodzajTransakcji);
                rodzajTransakcji.Aktywna = false;
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RodzajTransakcjiExists(int id)
        {
          return _context.RodzajTransakcji.Any(e => e.Id == id);
        }
    }
}
