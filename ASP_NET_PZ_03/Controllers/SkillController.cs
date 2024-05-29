using ASP_NET_PZ_03.Models;
using ASP_NET_PZ_03.Models.Forms;
using ASP_NET_PZ_03.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using System.Reflection;

namespace ASP_NET_PZ_03.Controllers
{
    public class SkillController : Controller
    {
        private readonly IObjectCollectionStorage<List<Skill>> _skillStorage;
        private readonly LocalUploadedFileStorage _fileStorage;

        public SkillController(IObjectCollectionStorage<List<Skill>> storage, LocalUploadedFileStorage fileStorage)
        {
            _skillStorage = storage;
            _fileStorage = fileStorage;
        }

        public IActionResult Index()
        {
            return View(_skillStorage.items);
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new SkillForm());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] SkillForm form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var model = new Skill();
            var random = new Random();
            model.Id = random.Next(1, 1000);
            model.Title = form.Title;

            if (form.Image != null)
            {
                model.Image = await _fileStorage.SaveUploadedFileAsync(form.Image);
            }

            _skillStorage.items.Add(model);
            await _skillStorage.SaveAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = _skillStorage.items.FirstOrDefault(x => x.Id == id);

            if (model.Image != null)
            {
                _fileStorage.DeleteUploadedFile(model.Image);
            }

            _skillStorage.items.Remove(model);
            await _skillStorage.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _skillStorage.items.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            ViewData["Skill"] = model;
            return View(new SkillForm
            {
                Title = model.Title,
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] SkillForm form)
        {
            var model = _skillStorage.items.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                return NotFound();
            }

            if (!ModelState.IsValid)
            {
                ViewData["Skill"] = model;
                return View(form);
            }

            model.Title = form.Title;

            if (form.Image != null)
            {
                if (model.Image != null)
                {
                    _fileStorage.DeleteUploadedFile(model.Image);
                }
                model.Image = await _fileStorage.SaveUploadedFileAsync(form.Image);
            }

            await _skillStorage.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
