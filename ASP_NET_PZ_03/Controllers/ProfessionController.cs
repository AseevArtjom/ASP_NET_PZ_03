using ASP_NET_PZ_03.Models;
using ASP_NET_PZ_03.Models.Forms;
using ASP_NET_PZ_03.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ASP_NET_PZ_03.Controllers
{
    [Authorize]
    public class ProfessionController : Controller
    {
        private readonly LocalUploadedFileStorage _fileStorage;
        private readonly SiteContext _siteContext;

        public ProfessionController(SiteContext siteContext, LocalUploadedFileStorage storage)
        {
            _siteContext = siteContext;
            _fileStorage = storage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _siteContext
                .Professions
                .Include(i => i.Image)
                .ToListAsync()
                );
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
                ViewBag.Professions = new SelectList(_siteContext.Professions, "Id", "Name");
                return View(form);
            }

            var info = new Profession();

            info.Name = form.Name;
            _siteContext.Add(info);

            if (form.Image != null)
            {
                info.Image = await _fileStorage.SaveUploadedFileAsync(form.Image);
            }

            await _siteContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = _siteContext.Professions.First(x => x.Id == id);

            if (model.Image != null)
            {
                _fileStorage.DeleteUploadedFile(model.Image); ;
            }

            _siteContext.Remove(model);
            await _siteContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var model = _siteContext.Professions.First(x => x.Id == id);
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
            var model = _siteContext.Professions.First(x => x.Id == id);
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

            await _siteContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

    }
}
