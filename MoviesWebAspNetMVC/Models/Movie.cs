using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesWebAspNetMVC.Models;

public partial class Movie
{
    public int Id { get; set; }
    [Display(Name = "Назва")]
    public string Name { get; set; } = null!;
    [Display(Name = "Про фільм")]
    public string Description { get; set; } = null!;
    [Display(Name = "Автор")]
    public int AuthorId { get; set; }
    [Display(Name = "Країна")]
    public int CountryId { get; set; }
    [Display(Name = "Жанр")]
    public int GenreId { get; set; }
    [Display(Name = "Картинка...")]
    public string PicturePath { get; set; } = null!;
    [Display(Name = "Автор")]
    public virtual Author Author { get; set; } = null!;
    [Display(Name = "Країна")]
    public virtual Country Country { get; set; } = null!;
    [Display(Name = "Жанр")]
    public virtual Genre Genre { get; set; } = null!;
    public virtual ICollection<Recension> Recensions { get; } = new List<Recension>();
}
