using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Study_Mate.Models;

namespace Study_Mate.Controllers
{
    public class LRController : Controller
    {
        private readonly ApplicationDbContext _context;

        public LRController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: LR
        public async Task<IActionResult> Index()
        {
            var users = _context.Users;
            return users != null ? View(await users.ToListAsync()) : Problem("Entity set 'ApplicationDbContext.Users' is null.");
        }

        // GET: LR/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.LRs == null)
            {
                return NotFound();
            }

            var lR = await _context.LRs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lR == null)
            {
                return NotFound();
            }

            return View(lR);
        }

        // GET: LR/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: LR/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Email")] LR lR)
        {
            if (ModelState.IsValid)
            {
                _context.Add(lR);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(lR);
        }

        // GET: LR/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.LRs == null)
            {
                return NotFound();
            }

            var lR = await _context.LRs.FindAsync(id);
            if (lR == null)
            {
                return NotFound();
            }
            return View(lR);
        }

        // POST: LR/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Email")] LR lR)
        {
            if (id != lR.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(lR);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!LRExists(lR.Id))
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
            return View(lR);
        }

        // GET: LR/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.LRs == null)
            {
                return NotFound();
            }

            var lR = await _context.LRs
                .FirstOrDefaultAsync(m => m.Id == id);
            if (lR == null)
            {
                return NotFound();
            }

            return View(lR);
        }

        // POST: LR/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.LRs == null)
            {
                return Problem("Entity set 'ApplicationDbContext.LRs'  is null.");
            }
            var lR = await _context.LRs.FindAsync(id);
            if (lR != null)
            {
                _context.LRs.Remove(lR);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool LRExists(int id)
        {
          return (_context.LRs?.Any(e => e.Id == id)).GetValueOrDefault();
        }
    }
}
