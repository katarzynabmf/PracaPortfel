using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Portfel.Data;
using Portfel.Data.Data;

namespace Portfel.Intranet.Controllers
{
    public class TransakcjaNewController : Controller
    {
         private readonly PortfelContext _context;

        public TransakcjaNewController(PortfelContext context)
        {
            _context = context;
        }

        // GET: Transakcja
        public async Task<IActionResult> Index()
        {
            var portfelContext = _context.TransakcjeNew
                .Include(t => t.Aktywo)
                .Include(t=>t.Portfel).ToListAsync();
            return View(await portfelContext);
        }

        // GET: Transakcja/Create
        public IActionResult Create()
        {
            ViewData["PortfelId"] = new SelectList(_context.Portfele, "Id", "Nazwa");
            ViewData["AktywoId"] = new SelectList(_context.Aktywa, "Id", "Nazwa");
          //  ViewData["Kierunek"] = new SelectList(Kierunek, "Id", "Nazwa");
            ViewBag.Kierunki = Enum.GetNames(typeof(Kierunek)).ToList();
            return View();
        }

        // POST: Transakcja/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Date,PortfelId, AktywoId,Cena,Ilosc,Kierunek, Komentarz, Aktywna")] StworzTransakcjaRequest stworzTransakcja)
        {
            if (ModelState.IsValid)
            {
                _context.Add(new TransakcjaNew()

                {
                    DataTransakcji = stworzTransakcja.DataTransakcji,
                    PortfelId = stworzTransakcja.PortfelId,
                    AktywoId = stworzTransakcja.AktywoId,
                    Cena = Convert.ToDecimal(stworzTransakcja.Cena, Thread.CurrentThread.CurrentCulture),
                    Ilosc = stworzTransakcja.Ilosc,
                    Kierunek = stworzTransakcja.Kierunek,
                    Komentarz = stworzTransakcja.Komentarz,
                    Aktywna = true,
                }
                );
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AktywoId"] = new SelectList(_context.Aktywa, "Id", "Nazwa", stworzTransakcja.AktywoId);
           
            ViewData["PortfelId"] = new SelectList(_context.Portfele, "Id", "Nazwa", stworzTransakcja.PortfelId);
            return View(stworzTransakcja);
        }

        // GET: Transakcja/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.TransakcjeNew == null)
            {
                return NotFound();
            }

            var transakcja = await _context.TransakcjeNew.FindAsync(id);
            if (transakcja == null)
            {
                return NotFound();
            }
            ViewData["AktywoId"] = new SelectList(_context.Aktywa, "Id", "Nazwa", transakcja.AktywoId);
            ViewData["PortfelId"] = new SelectList(_context.Portfele, "Id", "Nazwa", transakcja.PortfelId);

            return View(transakcja);
        }

        // POST: Transakcja/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Date,PortfelId,AktywoId,Cena,Ilosc,Kierunek, Komentarz, Aktywna")] EdytujTransakcjaRequest edytujTransakcja)
        {
            if (id != edytujTransakcja.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var transakcja = await _context.TransakcjeNew.FindAsync(id);
                    if (transakcja == null)
                    {
                        return NotFound();
                    }

                    transakcja.DataTransakcji = edytujTransakcja.DataTransakcji;
                    transakcja.PortfelId = edytujTransakcja.PortfelId;
                    transakcja.AktywoId = edytujTransakcja.AktywoId;
                    transakcja.Cena = Convert.ToDecimal(edytujTransakcja.Cena, Thread.CurrentThread.CurrentCulture);
                    transakcja.Ilosc = edytujTransakcja.Ilosc;
                    transakcja.Kierunek = edytujTransakcja.Kierunek;
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
            ViewData["RodzajOplatyId"] = new SelectList(_context.Portfele, "Id", "Nazwa", edytujTransakcja.PortfelId);
            ViewData["RodzajTransakcjiId"] = new SelectList(_context.Aktywa, "Id", "Nazwa", edytujTransakcja.AktywoId);
            return View(edytujTransakcja);
        }

        // GET: Transakcja/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.TransakcjeNew == null)
            {
                return NotFound();
            }

            var transakcja = await _context.TransakcjeNew
                .Include(t => t.Portfel)
                .Include(t => t.Aktywo)
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
            if (_context.TransakcjeNew == null)
            {
                return Problem("Entity set 'PortfelContext.Transakcja'  is null.");
            }
            var transakcja = await _context.TransakcjeNew.FindAsync(id);
            if (transakcja != null)
            {
                transakcja.Aktywna = false;
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TransakcjaExists(int id)
        {
            return _context.TransakcjeNew.Any(e => e.Id == id);
        }
    }
}
