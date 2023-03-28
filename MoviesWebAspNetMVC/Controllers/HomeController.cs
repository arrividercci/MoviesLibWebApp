using Microsoft.AspNetCore.Mvc;
using MoviesWebAspNetMVC.Models;
using MoviesWebAspNetMVC.Services;
using System.Diagnostics;

namespace MoviesWebAspNetMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        private readonly ExcelService _exelService;

        public HomeController(ILogger<HomeController> logger, ExcelService exelService)
        {
            _exelService = exelService;
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}