using ASP_NET_PZ_03.Models;
using ASP_NET_PZ_03.Models.Forms;
using ASP_NET_PZ_03.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace ASP_NET_PZ_03.Controllers
{
    public class ProfessionController : Controller
    {
        private readonly IObjectCollectionStorage<List<Profession>> _professionStorage;
        private readonly LocalUploadedFileStorage _fileStorage;

        private readonly SiteContext _siteContext;

        public ProfessionController(IObjectCollectionStorage<List<Profession>> storage, LocalUploadedFileStorage fileStorage)
        {
            _professionStorage = storage;
            _fileStorage = fileStorage;
        }

        public IActionResult Index()
        {
            return View(_professionStorage.items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new ProfessionForm());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] ProfessionForm form)
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

            if (form.Image != null)
            {
                info.Image = await _fileStorage.SaveUploadedFileAsync(form.Image);
            }

            _professionStorage.items.Add(info);
            await _professionStorage.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = _professionStorage.items.FirstOrDefault(x => x.Id == id);

            if (model.Image != null)
            {
                _fileStorage.DeleteUploadedFile(model.Image);
            }

            _professionStorage.items.Remove(model);
            await _professionStorage.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _professionStorage.items.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            ViewData["Profession"] = model;
            return View(new ProfessionForm
            {
                Name = model.Name,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] ProfessionForm form)
        {
            var model = _professionStorage.items.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewData["Profession"] = model;
                return View(form);
            }

            model.Name = form.Name;

            if (form.Image != null)
            {
                if (model.Image != null)
                {
                    _fileStorage.DeleteUploadedFile(model.Image);
                }
                model.Image = await _fileStorage.SaveUploadedFileAsync(form.Image);
            }

            await _professionStorage.SaveAsync();
            return RedirectToAction("Index");
        }

    }
}
