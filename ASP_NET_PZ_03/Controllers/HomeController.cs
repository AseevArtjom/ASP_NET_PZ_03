using ASP_NET_PZ_03.Models;
using ASP_NET_PZ_03.Models.Forms;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;

namespace ASP_NET_PZ_03.Controllers
{
    public class HomeController : Controller
    {
        private readonly SiteContext _siteContext;

        public HomeController(SiteContext siteContext)
        {
            _siteContext = siteContext;
        }  
        
        public async Task<IActionResult> Index([FromForm] HomeSearchForm form)
        {
            ViewData["SearchForm"] = form;
            if (form.Text == null)
            {
                form.Text = String.Empty;
            }

            var models = await _siteContext
                .Infos
                .Include(i => i.Images)
                .Include(p => p.Profession)
                .Include(r => r.Reviews)
                .Include(s => s.Skills)
                .ThenInclude(s => s.Skill)
                .ThenInclude(i => i.Image)
                .Where(x => x.FirstName.Contains(form.Text)
                || x.Description.Contains(form.Text)
                || EF.Functions.Like(x.LastName, $"%{form.Text}%")
                || EF.Functions.Like(x.Profession.Name, $"%{form.Text}%")
                )
                .ToListAsync();

            return View(models);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        public  IActionResult AboutMe()
        {
            

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