using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Study_Mate.Models;

namespace Study_Mate.Controllers
{
    [Authorize]
    public class SubjectController : Controller
    {
        //to interact with db
        private readonly ApplicationDbContext _context;

        public SubjectController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Subject
        public async Task<IActionResult> Index()
        {
              return _context.Subjects != null ? 
                          View(await _context.Subjects.ToListAsync()) :
                          Problem("Entity set 'ApplicationDbContext.Subjects'  is null.");
        }


        // GET: Subject/Changes
        public IActionResult Changes(int id=0)
        {
            if (id == 0)
                return View(new Subject());
            else
                return View(_context.Subjects.Find(id));
        }

        // POST: Subject/Changes
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Changes([Bind("SubjectId,Title,Icon,Type")] Subject subject)
        {
            if (ModelState.IsValid)
            {
                if (subject.SubjectId == 0)
                    _context.Add(subject);
                else
                    _context.Update(subject);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(subject);
        }


        // POST: Subject/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Subjects == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Subjects'  is null.");
            }
            var subject = await _context.Subjects.FindAsync(id);
            if (subject != null)
            {
                _context.Subjects.Remove(subject);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
