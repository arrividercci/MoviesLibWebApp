using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DocumentFormat.OpenXml.Office2021.PowerPoint.Comment;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using MoviesWebAspNetMVC.Models;
using MoviesWebAspNetMVC.Services;
using MoviesWebAspNetMVC.ViewModels;

namespace MoviesWebAspNetMVC.Controllers
{
    public class MoviesController : Controller
    {
        private readonly DbmyMoviesContext _context;
        private readonly IFileSaverService _saver;
        private readonly ExcelService _exelService;
        private readonly UserManager<User> _userManager;
        public MoviesController(DbmyMoviesContext context, IFileSaverService saver, ExcelService exelService, UserManager<User> userManager)
        {
            _userManager = userManager;
            _exelService = exelService;
            _context = context;
            _saver = saver;
        }

        // GET: Movies
        public async Task<IActionResult> Index()
        {
            var dbmovieContext = _context.Movies.Include(m => m.Author).Include(m => m.Country).Include(m => m.Genre).Include(m => m.Recensions);
            return View(await dbmovieContext.ToListAsync());
        }

        // GET: Movies/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Author)
                .Include(m => m.Country)
                .Include(m => m.Genre)
                .Include(m => m.Recensions)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }
            var temp = _context.UsersMarkedMovies.Include(u => u.Recencion).Where(x => movie.Recensions.Select(r => r.Id).Contains(x.Recencion.Id));

            var allUsers = await _userManager.Users.ToListAsync();
            List<RecensionViewModel> comments = new List<RecensionViewModel>();
            foreach (var t in temp)
            {
                var user = allUsers.Where(u => u.UserName.ToCharArray().Select(c => (int)c).Sum() == t.UserId).FirstOrDefault();
                if (user != null)
                {
                    RecensionViewModel viewModel = new RecensionViewModel()
                    {
                        UserName = user.UserName,
                        Comment = t.Recencion.Comment,
                        RateId = t.Recencion.RateId,
                    };
                    comments.Add(viewModel);
                }
            }
            ViewData["Comments"] = comments;
            return View(movie);
        }

        // GET: Movies/Create
        [Authorize(Roles="admin")]
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Surname");
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name");
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name");
            return View();
        }

        // POST: Movies/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Create(MovieViewModel movieView)
        {
            Movie movie = new Movie()
            {
                Id = movieView.Id,
                Name = movieView.Name,
                Description = movieView.Description,
                CountryId = movieView.CountryId,
                AuthorId = movieView.AuthorId,
                GenreId = movieView.GenreId,
                Author = movieView.Author,
                Genre = movieView.Genre,
                Country = movieView.Country,
                PicturePath = await _saver.SaveAsync(movieView.Image, "wwwroot\\images\\")
            };
            if (ModelState.IsValid)
            {
                _context.Add(movie);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Surname", movie.AuthorId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", movie.CountryId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            return View(movieView);
        }

        // GET: Movies/Edit/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies.FindAsync(id);
            if (movie == null)
            {
                return NotFound();
            }
            MovieViewModel movieView = new MovieViewModel()
            {
                Id = movie.Id,
                Name = movie.Name,
                Description = movie.Description,
                CountryId = movie.CountryId,
                AuthorId = movie.AuthorId,
                GenreId = movie.GenreId,
                Author = movie.Author,
                Genre = movie.Genre,
                Country = movie.Country,
                ImagePath = movie.PicturePath,
            };
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Surname", movie.AuthorId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", movie.CountryId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            return View(movieView);
        }

        // POST: Movies/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Edit(int id, MovieViewModel movieView)
        {
            string picturePath;
            if (movieView.Image == null) picturePath = movieView.ImagePath;
            else picturePath = await _saver.SaveAsync(movieView.Image, "wwwroot\\images\\");
            Movie movie = new Movie()
            {
                Id = movieView.Id,
                Name = movieView.Name,
                Description = movieView.Description,
                CountryId = movieView.CountryId,
                AuthorId = movieView.AuthorId,
                GenreId = movieView.GenreId,
                Author = movieView.Author,
                Genre = movieView.Genre,
                Country = movieView.Country,
                PicturePath = picturePath,

            };
            if (id != movie.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(movie);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!MovieExists(movie.Id))
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
            ViewData["AuthorId"] = new SelectList(_context.Authors, "Id", "Surname", movie.AuthorId);
            ViewData["CountryId"] = new SelectList(_context.Countries, "Id", "Name", movie.CountryId);
            ViewData["GenreId"] = new SelectList(_context.Genres, "Id", "Name", movie.GenreId);
            return View(movie);
        }

        // GET: Movies/Delete/5
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Movies == null)
            {
                return NotFound();
            }

            var movie = await _context.Movies
                .Include(m => m.Author)
                .Include(m => m.Country)
                .Include(m => m.Genre)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (movie == null)
            {
                return NotFound();
            }

            return View(movie);
        }

        // POST: Movies/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Movies == null)
            {
                return Problem("Entity set 'DbmovieContext.Movies'  is null.");
            }
            var movie = await _context.Movies.FindAsync(id);
            if (movie != null)
            {
                _context.Movies.Remove(movie);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool MovieExists(int id)
        {
            return _context.Movies.Any(e => e.Id == id);
        }
        public async Task<IActionResult> ExcelMethod()
        {
            var movies = await _context.Movies.ToListAsync();
            ViewData["MoviesId"] = new SelectList(movies, "Id", "Name");
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> ImportXL(IFormFile fileExcel)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    await _exelService.GetMoviesByGenreAsync(fileExcel);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return RedirectToAction(nameof(Index));
        }
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> ExportXL(List<int> moviesIds)
        {
            if (ModelState.IsValid)
            {
                var movies = await _context.Movies.Where(m => moviesIds.Contains(m.Id)).ToListAsync();
                try
                {
                    return await _exelService.CreateExelFileByGenresAsync(movies);
                }
                catch (Exception)
                {
                    return BadRequest();
                }
            }
            return RedirectToAction(nameof(ExcelMethod));
        }
    }
}
