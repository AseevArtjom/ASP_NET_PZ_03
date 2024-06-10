using ASP_NET_PZ_03.Models;
using ASP_NET_PZ_03.Models.Forms;
using ASP_NET_PZ_03.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace ASP_NET_PZ_03.Controllers
{
    public class SkillController : Controller
    {
        private readonly SiteContext _siteContext;
        private readonly LocalUploadedFileStorage _fileStorage;

        public SkillController(LocalUploadedFileStorage fileStorage,SiteContext sitecontext)
        {
            _siteContext = sitecontext;
            _fileStorage = fileStorage;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _siteContext
                .Skills
                .Include(i => i.Image)
                .ToListAsync()
                );
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
            model.Title = form.Title;

            if (form.Image != null)
            {
                model.Image = await _fileStorage.SaveUploadedFileAsync(form.Image);
            }

            await _siteContext.Skills.AddAsync(model);
            await _siteContext.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _siteContext.Skills.Include(i => i.Image).FirstAsync(x => x.Id == id);

            if (model.Image != null)
            {
                _fileStorage.DeleteUploadedFile(model.Image);
            }
            _siteContext.Remove(model);
            await _siteContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _siteContext.Skills.Include(i => i.Image).FirstAsync(x => x.Id == id);

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
            var model = await _siteContext.Skills.Include(i => i.Image).FirstAsync(x => x.Id == id);
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

            await _siteContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
