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
    public class TrackController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TrackController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Track
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Tracks.Include(t => t.Subject);
            return View(await applicationDbContext.ToListAsync());
        }


        // GET: Track/Changes
        public IActionResult Changes(int id=0)
        {
            AllSubjects();
            if (id == 0)
                return View(new Track());
            else
                return View(_context.Tracks.Find(id));
            
        }

        // POST: Track/Changes
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Changes([Bind("TrackId,SubjectId,Amount,Note,Date")] Track track)
        {
            if (ModelState.IsValid)
            {
                if(track.TrackId == 0)
                    _context.Add(track);
                else
                    _context.Update(track);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            AllSubjects();
            return View(track);
        }


        // POST: Track/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Tracks == null)
            {
                return Problem("Entity set 'ApplicationDbContext.Tracks'  is null.");
            }
            var track = await _context.Tracks.FindAsync(id);
            if (track != null)
            {
                _context.Tracks.Remove(track);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
        [NonAction]
        public void AllSubjects()
        {
            var SubjectCollection = _context.Subjects.ToList();
            Subject DefaultSubject = new Subject() { SubjectId = 0, Title = "Choose a subject" };
            SubjectCollection.Insert(0, DefaultSubject);
            ViewBag.Subjects = SubjectCollection;
        }
    }
}
