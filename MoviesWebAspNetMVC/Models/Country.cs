using DocumentFormat.OpenXml.Wordprocessing;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesWebAspNetMVC.Models;

public partial class Country
{
    public int Id { get; set; }
    [Display(Name = "Назва")]
    [Required(ErrorMessage = "Країна має мати назву")]
    public string Name { get; set; } = null!;

    public virtual ICollection<Movie> Movies { get; } = new List<Movie>();
}
