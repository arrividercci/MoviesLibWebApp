using MoviesWebAspNetMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviesWebAspNetMVC.ViewModels
{
    public partial class MovieViewModel
    {
        public int Id { get; set; }
        [Display(Name = "Назва")]
        [Required(ErrorMessage = "Назва не може бути пустою.")]
        public string Name { get; set; } = null!;
        [Display(Name = "Про фільм")]
        [Required(ErrorMessage = "Фільм має мати описання.")]
        public string Description { get; set; } = null!;
        [Display(Name = "Автор")]
        public int AuthorId { get; set; }
        [Display(Name = "Країна")]
        public int CountryId { get; set; }
        [Display(Name = "Жанр")]
        public int GenreId { get; set; }
        public string ImagePath { get; set; } = null;
        [Display(Name = "Постер")]
        public IFormFile Image { get; set; }
        [Display(Name = "Автор")]
        public virtual Author Author { get; set; } = null!;
        [Display(Name = "Країна")]
        public virtual Country Country { get; set; } = null!;
        [Display(Name = "Жанр")]
        public virtual Genre Genre { get; set; } = null!;
    }
}
