using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfel.Data;
using Portfel.Data.Data;

namespace Portfel.Intranet.Controllers
{
    public class RodzajOplatyController : Controller
    {
        private readonly PortfelContexts _context;

        public RodzajOplatyController(PortfelContexts context)
        {
            _context = context;
        }

        // GET: RodzajOplaty
        public async Task<IActionResult> Index()
        {
              return View(await _context.RodzajOplaty.ToListAsync());
        }

        // GET: RodzajOplaty/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.RodzajOplaty == null)
            {
                return NotFound();
            }

            var rodzajOplaty = await _context.RodzajOplaty
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rodzajOplaty == null)
            {
                return NotFound();
            }

            return View(rodzajOplaty);
        }

        // GET: RodzajOplaty/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: RodzajOplaty/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Nazwa,Promowany, Aktywna")] RodzajOplaty rodzajOplaty)
        {
            if (ModelState.IsValid)
            {
                _context.Add(rodzajOplaty);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(rodzajOplaty);
        }

        // GET: RodzajOplaty/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.RodzajOplaty == null)
            {
                return NotFound();
            }

            var rodzajOplaty = await _context.RodzajOplaty.FindAsync(id);
            if (rodzajOplaty == null)
            {
                return NotFound();
            }
            return View(rodzajOplaty);
        }

        // POST: RodzajOplaty/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Nazwa,Promowany, Aktywna")] RodzajOplaty rodzajOplaty)
        {
            if (id != rodzajOplaty.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(rodzajOplaty);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RodzajOplatyExists(rodzajOplaty.Id))
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
            return View(rodzajOplaty);
        }

        // GET: RodzajOplaty/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.RodzajOplaty == null)
            {
                return NotFound();
            }

            var rodzajOplaty = await _context.RodzajOplaty
                .FirstOrDefaultAsync(m => m.Id == id);
            if (rodzajOplaty == null)
            {
                return NotFound();
            }

            return View(rodzajOplaty);
        }

        // POST: RodzajOplaty/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.RodzajOplaty == null)
            {
                return Problem("Entity set 'PortfelContext.RodzajOplaty'  is null.");
            }
            var rodzajOplaty = await _context.RodzajOplaty.FindAsync(id);
            if (rodzajOplaty != null)
            {
                _context.RodzajOplaty.Remove(rodzajOplaty);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RodzajOplatyExists(int id)
        {
          return _context.RodzajOplaty.Any(e => e.Id == id);
        }
    }
}
