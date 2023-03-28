using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Wordprocessing;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesWebAspNetMVC.Models;
using MoviesWebAspNetMVC.ViewModels;

namespace MoviesWebAspNetMVC.Controllers
{
    [Authorize(Roles = "admin, user")]
    public class UsersMarkedMoviesController : Controller
    {
        private readonly DbmyMoviesContext _context;

        public UsersMarkedMoviesController(DbmyMoviesContext context)
        {
            _context = context;
        }

        // GET: UsersMarkedMovies
        public async Task<IActionResult> Index()
        {
            int userId = User.Identity.Name.ToCharArray().Select(c => (int)c).Sum();
            var userRecensionIds = _context.UsersMarkedMovies.Where(u => u.UserId == userId).Select(u => u.RecencionId);
            var recensions = _context.Recensions.Where(r => userRecensionIds.Contains(r.Id)).Include(r => r.Movie).Include(r => r.Rate);
            return View(await recensions.ToListAsync());
        }

        // GET: UsersMarkedMovies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.UsersMarkedMovies == null)
            {
                return NotFound();
            }

            var usersMarkedMovie = await _context.UsersMarkedMovies
                .Include(u => u.Recencion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usersMarkedMovie == null)
            {
                return NotFound();
            }

            return View(usersMarkedMovie);
        }
        public async Task<IActionResult> CreateComment(int recensionId)
        {
            if (ModelState.IsValid)
            {
                UsersMarkedMovie usersMarkedMovie = new UsersMarkedMovie()
                {
                    RecencionId = recensionId,
                    UserId = User.Identity.Name.ToCharArray().Select(c => (int)c).Sum(),
                };
                await _context.AddAsync(usersMarkedMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            else
            {
                return BadRequest();
            }

        }

        // GET: UsersMarkedMovies/Create
        public IActionResult Create(int movieId)
        {
            RecensionViewModel recension = new RecensionViewModel()
            {
                MovieId = movieId,
                UserId = User.Identity.Name.ToCharArray().Select(c => (int)c).Sum(),
            };
            ViewData["Rates"] = new SelectList(_context.Rates, "Id", "Name");
            return View(recension);
        }

        // POST: UsersMarkedMovies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RecensionViewModel recensionViewModel)
        {
            if (ModelState.IsValid)
            {
                Recension recension = new Recension()
                {
                    MovieId = recensionViewModel.MovieId,
                    Comment = recensionViewModel.Comment,
                    RateId = recensionViewModel.RateId,
                };
                await _context.AddAsync(recension);
                await _context.SaveChangesAsync();

                UsersMarkedMovie usersMarkedMovie = new UsersMarkedMovie()
                {
                    RecencionId = recension.Id,
                    UserId = recensionViewModel.UserId,
                };
                await _context.AddAsync(usersMarkedMovie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Rates"] = new SelectList(_context.Rates, "Id", "Name");
            return View(recensionViewModel);
        }

        // GET: UsersMarkedMovies/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.UsersMarkedMovies == null)
            {
                return NotFound();
            }

            var usersMarkedMovie = await _context.UsersMarkedMovies.FindAsync(id);
            if (usersMarkedMovie == null)
            {
                return NotFound();
            }
            ViewData["RecencionId"] = new SelectList(_context.Recensions, "Id", "Id", usersMarkedMovie.RecencionId);
            return View(usersMarkedMovie);
        }

        // POST: UsersMarkedMovies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,UserId,RecencionId")] UsersMarkedMovie usersMarkedMovie)
        {
            if (id != usersMarkedMovie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(usersMarkedMovie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!UsersMarkedMovieExists(usersMarkedMovie.Id))
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
            ViewData["RecencionId"] = new SelectList(_context.Recensions, "Id", "Id", usersMarkedMovie.RecencionId);
            return View(usersMarkedMovie);
        }

        // GET: UsersMarkedMovies/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.UsersMarkedMovies == null)
            {
                return NotFound();
            }

            var usersMarkedMovie = await _context.UsersMarkedMovies
                .Include(u => u.Recencion)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (usersMarkedMovie == null)
            {
                return NotFound();
            }

            return View(usersMarkedMovie);
        }

        // POST: UsersMarkedMovies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.UsersMarkedMovies == null)
            {
                return Problem("Entity set 'DbmyMoviesContext.UsersMarkedMovies'  is null.");
            }
            var usersMarkedMovie = await _context.UsersMarkedMovies.FindAsync(id);
            if (usersMarkedMovie != null)
            {
                _context.UsersMarkedMovies.Remove(usersMarkedMovie);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool UsersMarkedMovieExists(int id)
        {
          return _context.UsersMarkedMovies.Any(e => e.Id == id);
        }
    }
}
