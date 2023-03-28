using System;
using System.Collections.Generic;

namespace MoviesWebAspNetMVC.Models;

public partial class Rate
{
    public int Id { get; set; }

    public string Name { get; set; } = null!;

    public virtual ICollection<Recension> Recensions { get; } = new List<Recension>();
}
