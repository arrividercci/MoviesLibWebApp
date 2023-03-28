using ClosedXML.Excel;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MoviesWebAspNetMVC.Models;

namespace MoviesWebAspNetMVC.Services
{
    public class ExcelService
    {
        private readonly DbmyMoviesContext _context;

        public ExcelService(DbmyMoviesContext context)
        {
            _context = context;
        }
        public async Task GetMoviesByGenreAsync(IFormFile exelFile)
        {

            if (exelFile != null)
            {
                using (var stream = exelFile.OpenReadStream())
                {
                    using (XLWorkbook workbook = new XLWorkbook(stream))
                    {
                        foreach (IXLWorksheet worksheet in workbook.Worksheets)
                        {
                            Genre genre;
                            var genres = await _context.Genres.Where(x => x.Name.Contains(worksheet.Name)).ToListAsync();

                            if (genres.Count > 0)
                            {
                                genre = genres[0];
                            }
                            else
                            {
                                genre = new Genre();
                                genre.Name = worksheet.Name;
                                await _context.AddAsync(genre);
                            }
                            foreach (IXLRow row in worksheet.RowsUsed().Skip(1))
                            {
                                Models.Author author;
                                Country country;
                                try
                                {
                                    Movie movie = new Movie();
                                    movie.Genre = genre;
                                    movie.Name = row.Cell(1).Value.ToString();
                                    movie.Description = row.Cell(2).Value.ToString();
                                    var authorFullName = row.Cell(3).Value.ToString().Split(" ");
                                    var authors = await _context.Authors.Where(a => a.Name.Equals(authorFullName[0]) && a.Surname.Equals(authorFullName[1])).ToListAsync();
                                    if (authors.Count > 0)
                                    {
                                        author = authors[0];
                                    }
                                    else
                                    {
                                        author = new Models.Author();
                                        author.Name = authorFullName[0];
                                        author.Surname = authorFullName[1];
                                        await _context.AddAsync(author);
                                    }
                                    movie.Author = author;
                                    var countryName = row.Cell(4).Value.ToString();
                                    var countries = await _context.Countries.Where(c => c.Name.Equals(countryName)).ToListAsync();
                                    if (countries.Count > 0)
                                    {
                                        country = countries[0];
                                    }
                                    else
                                    {
                                        country = new Country();
                                        country.Name = countryName;
                                        await _context.AddAsync(country);
                                    }
                                    movie.Country = country;
                                    movie.PicturePath = row.Cell(5).Value.ToString();
                                    var movies = _context.Movies.Where(x => x.Name == movie.Name && x.Description == movie.Description);

                                    if (movies.Count() > 0) continue;
                                    else await _context.AddAsync(movie);
                                }
                                catch (Exception ex)
                                {
                                    throw new Exception("Помилка при додаванні данних!", ex);
                                }
                            }
                        }
                    }
                }
                await _context.SaveChangesAsync();
            }

        }

        public async Task<FileContentResult> CreateExelFileByGenresAsync(List<Movie> movies)
        {
            using (XLWorkbook workbook = new XLWorkbook())
            {
                var genres = await _context.Genres.Include(g => g.Movies).ToListAsync();
                foreach (var genre in genres)
                {
                    if (movies.Select(m => m.GenreId).Contains(genre.Id))
                    {
                        var worksheet = workbook.Worksheets.Add(genre.Name);
                        worksheet.Cell("A1").Value = "Назва";
                        worksheet.Cell("B1").Value = "Опис";
                        worksheet.Cell("C1").Value = "Автор";
                        worksheet.Cell("D1").Value = "Країна";
                        worksheet.Cell("E1").Value = "Постер";
                        worksheet.Row(1).Style.Fill.BackgroundColor = XLColor.BabyBlue;
                        var moviesByGenre = movies.Where(m => m.GenreId == genre.Id);
                        int index = 0;
                        foreach (var movie in moviesByGenre)
                        {
                            worksheet.Cell(index + 2, 1).Value = movie.Name;
                            worksheet.Cell(index + 2, 2).Value = movie.Description;
                            var author = await _context.Authors.FirstOrDefaultAsync(a => a.Id == movie.AuthorId);
                            worksheet.Cell(index + 2, 3).Value = $"{author.Name} {author.Surname}";
                            var country = await _context.Countries.FirstOrDefaultAsync(c => c.Id == movie.CountryId);
                            worksheet.Cell(index + 2, 4).Value = country.Name;
                            worksheet.Cell(index + 2, 5).Value = movie.PicturePath;
                            index++;
                        }
                    }
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    stream.Flush();
                    return new FileContentResult(stream.ToArray(), "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
                    {
                        FileDownloadName = $"MoviesByGenre_{DateTime.UtcNow.ToShortDateString()}.xlsx"
                    };
                }
            }
        }
    }
}
