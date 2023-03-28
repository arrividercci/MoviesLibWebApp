using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesWebAspNetMVC.Models;
using MoviesWebAspNetMVC.ViewModels;

namespace MoviesWebAspNetMVC.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class RecensionsController : Controller
    {
        private readonly DbmyMoviesContext _context;

        public RecensionsController(DbmyMoviesContext context)
        {
            _context = context;
        }

        // GET: Recensions
        public async Task<IActionResult> Index()
        {
            var dbmyMoviesContext = _context.Recensions.Include(r => r.Movie).Include(r => r.Rate);
            return View(await dbmyMoviesContext.ToListAsync());
        }

        // GET: Recensions/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Recensions == null)
            {
                return NotFound();
            }

            var recension = await _context.Recensions
                .Include(r => r.Movie)
                .Include(r => r.Rate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recension == null)
            {
                return NotFound();
            }

            return View(recension);
        }

        // GET: Recensions/Create
        public async Task<IActionResult> Create(int? movieId)
        {
            if (movieId == null) return BadRequest();

            int userId = User.Identity.Name.ToCharArray().Select(c => (int)c).Sum();

            var recensionsMovieId = await _context.UsersMarkedMovies.Where(u => u.UserId == userId).Include(u => u.Recencion).Select(u => u.Recencion.MovieId).ToListAsync();

            if (recensionsMovieId.Contains((int)movieId)) 
            {
                ModelState.AddModelError(string.Empty, "У вас вже є рецензія до цього фільму");
                return RedirectToAction("Index", "UsersMarkedMovies"); 
            }

            RecensionViewModelDemo recension = new RecensionViewModelDemo()
            {
                MovieId = (int)movieId
            };
            ViewData["RateId"] = new SelectList(_context.Rates, "Id", "Name");
            return View(recension);
        }

        // POST: Recensions/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecensionViewModelDemo recensionView)
        {
            Recension recension = new Recension()
            {
                Id = recensionView.Id,
                MovieId = recensionView.MovieId,
                RateId = recensionView.RateId,
                Comment = recensionView.Comment,
                Rate = recensionView.Rate,
                Movie = recensionView.Movie,
            };
            if (ModelState.IsValid)
            {
                _context.Add(recension);
                await _context.SaveChangesAsync();
                return RedirectToAction("CreateComment", "UsersMarkedMovies", new {recensionId = recension.Id});
            }
            ViewData["RateId"] = new SelectList(_context.Rates, "Id", "Id", recension.RateId);
            return View(recensionView);
        }

        // GET: Recensions/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Recensions == null)
            {
                return NotFound();
            }

            var recension = await _context.Recensions.FindAsync(id);
            if (recension == null)
            {
                return NotFound();
            }
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", recension.MovieId);
            ViewData["RateId"] = new SelectList(_context.Rates, "Id", "Id", recension.RateId);
            return View(recension);
        }

        // POST: Recensions/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,MovieId,RateId,Comment")] Recension recension)
        {
            if (id != recension.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(recension);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RecensionExists(recension.Id))
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
            ViewData["MovieId"] = new SelectList(_context.Movies, "Id", "Id", recension.MovieId);
            ViewData["RateId"] = new SelectList(_context.Rates, "Id", "Id", recension.RateId);
            return View(recension);
        }

        // GET: Recensions/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Recensions == null)
            {
                return NotFound();
            }

            var recension = await _context.Recensions
                .Include(r => r.Movie)
                .Include(r => r.Rate)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (recension == null)
            {
                return NotFound();
            }

            return View(recension);
        }

        // POST: Recensions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Recensions == null)
            {
                return Problem("Entity set 'DbmyMoviesContext.Recensions'  is null.");
            }
            var recension = await _context.Recensions.FindAsync(id);
            if (recension != null)
            {
                _context.Recensions.Remove(recension);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool RecensionExists(int id)
        {
          return _context.Recensions.Any(e => e.Id == id);
        }
    }
}
