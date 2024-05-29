using ASP_NET_PZ_03.Models;
using ASP_NET_PZ_03.Models.Forms;
using ASP_NET_PZ_03.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_NET_PZ_03.Controllers
{
    public class InfoController : Controller
    {
        private readonly IObjectCollectionStorage<List<Info>> _infoStorage;
        private readonly IObjectCollectionStorage<List<Profession>> _professionStorage;
        private readonly IObjectCollectionStorage<List<Skill>> _skillStorage;

        public InfoController(IObjectCollectionStorage<List<Info>> infoStorage, IObjectCollectionStorage<List<Profession>> professionStorage, IObjectCollectionStorage<List<Skill>> skillStorage)
        {
            _infoStorage = infoStorage;
            _professionStorage = professionStorage;
            _skillStorage = skillStorage;
        }

        public IActionResult Index()
        {
            return View(_infoStorage.items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Professions = new SelectList(_professionStorage.items, "Id", "Name");
            return View(new InfoForm());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] InfoForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Professions = new SelectList(_professionStorage.items, "Id", "Name");
                return View(form);
            }

            var info = new Info();
            var random = new Random();
            info.Id = random.Next(1, 1000);
            form.Fill(info);

            info.Profession = _professionStorage.items.FirstOrDefault(x => x.Id == form.ProfessionId);
            info.Skills = form.Skills;

            _infoStorage.items.Add(info);
            await _infoStorage.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = _infoStorage.items.FirstOrDefault(x => x.Id == id);
            _infoStorage.items.Remove(model);
            await _infoStorage.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var info = _infoStorage.items.FirstOrDefault(x => x.Id == id);
            if (info == null)
            {
                return NotFound();
            }
            ViewBag.Professions = new SelectList(_professionStorage.items, "Id", "Name", info.ProfessionId);
            ViewData["Skills"] = info.Skills;
            var form = new InfoForm(info);
            return View(form);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] InfoForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Professions = new SelectList(_professionStorage.items, "Id", "Name", form.ProfessionId);
                return View(form);
            }

            var info = _infoStorage.items.First(x => x.Id == id);
            form.Fill(info);

            await _infoStorage.SaveAsync();
            return RedirectToAction("Index");
        }

        public IActionResult Show(int id)
        {
            var info = _infoStorage.items.FirstOrDefault(x => x.Id == id);
            if (info == null)
            {
                return NotFound();
            }

            ViewBag.Profession = _professionStorage.items.FirstOrDefault(p => p.Id == info.ProfessionId)?.Name;
            return View(info);
        }
    }
}
