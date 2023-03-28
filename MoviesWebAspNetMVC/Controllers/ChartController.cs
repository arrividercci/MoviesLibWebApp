using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesWebAspNetMVC.Models;

namespace MoviesWebAspNetMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartController : ControllerBase
    {
        private readonly DbmyMoviesContext _context;

        public ChartController(DbmyMoviesContext context)
        {
            _context = context;
        }

        [HttpGet("JsonMoviesByGenreData")]
        public async Task<JsonResult> JsonMoviesByGenresDataAsync()
        {
            var genres = await _context.Genres.Include(genre => genre.Movies).ToListAsync();

            List<object> genreMovie = new List<object>();

            genreMovie.Add(new[] { "Жанр", "Кількість фільмів" });

            foreach (var genre in genres)
            {
                genreMovie.Add(new object[] { genre.Name, genre.Movies.Count() });
            }

            return new JsonResult(genreMovie);
        }

        [HttpGet("JsonMoviesByAuthorsData")]
        public async Task<JsonResult> JsonMoviesByAuthorsDataAsync()
        {
            var authors = await _context.Authors.Include(author => author.Movies).ToListAsync();

            List<object> authorsByMovies = new List<object>();

            authorsByMovies.Add(new[] { "Автор", "Фільми" });

            foreach (var author in authors)
            {
                authorsByMovies.Add(new object[] { $"{author.Name[0]}.{author.Surname}", author.Movies.Count() });
            }

            return new JsonResult(authorsByMovies);
        }

        [HttpGet("JsonMoviesByCountryData")]
        public async Task<JsonResult> JsonMoviesByCountryData()
        {
            var countries = await _context.Countries.Include(country => country.Movies).ToListAsync();

            List<object> countriesByMovies = new List<object>();

            countriesByMovies.Add(new[] { "Країна", "Фільми" });

            foreach (var county in countries)
            {
                countriesByMovies.Add(new object[] { county.Name, county.Movies.Count() });
            }

            return new JsonResult(countriesByMovies);
        }
    }
}
