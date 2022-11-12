using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfel.Data;
using Portfel.Data.Data;
using Portfel.Data.Migrations;

namespace Portfel.Intranet.Controllers
{
    public class AktualnoscController : Controller
    {
        private readonly PortfelContexts _context;

        public AktualnoscController(PortfelContexts context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Aktualnosc.ToListAsync());
        }
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Pozycja,Tytul,Tresc,FotoUrl, Aktywna")] Aktualnosc aktualnosc)
        {
            if (ModelState.IsValid)
            {
                aktualnosc.DataDodania=DateTime.Now;
                _context.Add(aktualnosc);
 
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(aktualnosc);
        }
        // GET: Aktualnosci/Delete/5
        public async Task<IActionResult> UsuwaniePotwierdzone(int id)
        {
            var aktualnosc = await _context.Aktualnosc.FindAsync(id);
            _context.Aktualnosc.Remove(aktualnosc);
            //aktualnosc.Aktywna = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Aktualnosc == null)
            {
                return NotFound();
            }

            var aktualnosc = await _context.Aktualnosc.FindAsync(id);
            if (aktualnosc == null)
            {
                return NotFound();
            }
            return View(aktualnosc);
        }
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Pozycja,Tytul,Tresc,FotoUrl,DataDodania, Aktywna")] Aktualnosc aktualnosc)
        {
            if (id != aktualnosc.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(aktualnosc);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AktualnoscExists(aktualnosc.Id))
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
            return View(aktualnosc);
        }
        private bool AktualnoscExists(int id)
        {
            return _context.Aktualnosc.Any(e => e.Id == id);
        }
     
    }
}