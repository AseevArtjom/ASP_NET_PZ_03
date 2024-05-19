using ASP_NET_PZ_03.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace ASP_NET_PZ_03.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
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

        public  IActionResult AboutMe()
        {
            

            return View();
        }

        public IActionResult Skills()
        {
            var model = new Skills[] { 
                new Skills {Name = "C#/C++",Level = 60},
                new Skills { Name = "SQL", Level = 70 },
                new Skills {Name = "JS",Level = 80 },
                new Skills {Name = "React/Angular",Level = 60 } 
            };

            ViewData["Skills"] = model;

            return View();
        }

        public IActionResult Form()
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