using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesWebAspNetMVC.Models;

public partial class Author
{
    public int Id { get; set; }

    [Display(Name = "Ім'я")]
    [Required(ErrorMessage = "Автор має мати ім'я")]
    public string Name { get; set; } = null!;
    [Display(Name = "Прізвище")]
    [Required(ErrorMessage = "Автор має мати прізвище")]
    public string Surname { get; set; } = null!;

    public string FullName()
    {
        return $"{Name} {Surname}";
    }
    public virtual ICollection<Movie> Movies { get; } = new List<Movie>();
}