﻿using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Portfel.Data;
using Portfel.Data.Data;

namespace Portfel.Intranet.Controllers
{
    public class UzytkownikController : Controller
    {
        private readonly PortfelContext _context;

        public UzytkownikController(PortfelContext context)
        {
            _context = context;
        }

        // GET: Uzytkownik
        public async Task<IActionResult> Index()
        {
              return View(await _context.Uzytkownik.ToListAsync());
        }

        // GET: Uzytkownik/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Uzytkownik == null)
            {
                return NotFound();
            }

            var uzytkownik = await _context.Uzytkownik
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }

            return View(uzytkownik);
        }

        // GET: Uzytkownik/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Uzytkownik/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Imie,Haslo,Email,Aktywna,DataUtworzenia")] Uzytkownik uzytkownik)
        {
            if (ModelState.IsValid)
            {
                uzytkownik.DataUtworzenia = DateTime.Now;
                _context.Add(uzytkownik);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(uzytkownik);
        }

        // GET: Uzytkownik/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Uzytkownik == null)
            {
                return NotFound();
            }

            var uzytkownik = await _context.Uzytkownik.FindAsync(id);
            if (uzytkownik == null)
            {
                return NotFound();
            }
            return View(uzytkownik);
        }

        // POST: Uzytkownik/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Imie,Haslo,Email, Aktywna")] Uzytkownik uzytkownik)
        {
            if (id != uzytkownik.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(uzytkownik);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UzytkownikExists(uzytkownik.Id))
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
            return View(uzytkownik);
        }

        // GET: Uzytkownik/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Uzytkownik == null)
            {
                return NotFound();
            }

            var uzytkownik = await _context.Uzytkownik
                .FirstOrDefaultAsync(m => m.Id == id);
            if (uzytkownik == null)
            {
                return NotFound();
            }

            return View(uzytkownik);
        }

        // POST: Uzytkownik/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Uzytkownik == null)
            {
                return Problem("Entity set 'PortfelContext.Uzytkownik'  is null.");
            }
            var uzytkownik = await _context.Uzytkownik.FindAsync(id);
            if (uzytkownik != null)
            {
                uzytkownik.Aktywna = false;
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UzytkownikExists(int id)
        {
          return _context.Uzytkownik.Any(e => e.Id == id);
        }
    }
}
