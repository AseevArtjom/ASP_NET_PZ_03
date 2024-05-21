using ASP_NET_PZ_03.Models;
using ASP_NET_PZ_03.Models.Forms;
using ASP_NET_PZ_03.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP_NET_PZ_03.Controllers
{
    public class ProfessionController : Controller
    {
        private readonly IObjectCollectionStorage<List<Profession>> _professionStorage;

        public ProfessionController(IObjectCollectionStorage<List<Profession>> storage) 
        {
            _professionStorage = storage;
        }

        public IActionResult Index()
        {
            return View(_professionStorage.items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new Profession());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] Profession form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Professions = new SelectList(_professionStorage.items, "Id", "Name");
                return View(form);
            }

            var info = new Profession();
            var random = new Random();
            info.Id = random.Next(1, 1000);
            info.Name = form.Name;

            _professionStorage.items.Add(info);
            await _professionStorage.SaveAsync();
            return RedirectToAction("Index");
        }


    }
}
