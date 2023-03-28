using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace MoviesWebAspNetMVC.Models;

public partial class Recension
{
    public int Id { get; set; }

    public int MovieId { get; set; }

    public int RateId { get; set; }
    public string? Comment { get; set; }

    public virtual Movie Movie { get; set; } = null!;

    public virtual Rate Rate { get; set; } = null!;

    public virtual ICollection<UsersMarkedMovie> UsersMarkedMovies { get; } = new List<UsersMarkedMovie>();
}
