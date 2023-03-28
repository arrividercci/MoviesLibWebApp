using Microsoft.Build.Framework;
using MoviesWebAspNetMVC.Models;
using System.ComponentModel.DataAnnotations;

namespace MoviesWebAspNetMVC.ViewModels
{
    public class RecensionViewModelDemo
    {
        public int Id { get; set; }

        public int MovieId { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [Display(Name = "Оцінка")]
        public int RateId { get; set; }
        [System.ComponentModel.DataAnnotations.Required]
        [Display(Name = "Рецензія")]
        public string Comment { get; set; }

        public virtual Movie Movie { get; set; } = null!;

        public virtual Rate Rate { get; set; } = null!;

    }
}
