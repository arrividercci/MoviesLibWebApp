using System;
using System.Collections.Generic;

namespace MoviesWebAspNetMVC.Models;

public partial class UsersMarkedMovie
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int RecencionId { get; set; }

    public virtual Recension Recencion { get; set; } = null!;
}
