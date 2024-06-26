using ASP_NET_PZ_03.Models;
using ASP_NET_PZ_03.Models.Forms;
using ASP_NET_PZ_03.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Linq;
using System.Threading.Tasks;

namespace ASP_NET_PZ_03.Controllers
{
    [Authorize]
    public class InfoController : Controller
    {
        private readonly SiteContext _siteContext;
        private readonly LocalUploadedFileStorage _localUploadedFileStorage;

        public InfoController(LocalUploadedFileStorage localUploadedFileStorage, SiteContext siteContext)
        {
            _localUploadedFileStorage = localUploadedFileStorage;
            _siteContext = siteContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(
                await _siteContext
                .Infos
                .Include(p => p.Profession)
                .Include(i => i.Images)
                .Include(r => r.Reviews)
                .Include(s => s.Skills)
                .ThenInclude(s => s.Skill)
                .ThenInclude(i => i.Image)
                .ToListAsync());
        }

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Professions = new SelectList(_siteContext.Professions, "Id", "Name");
            return View(new InfoForm());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] InfoForm form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Professions = new SelectList(_siteContext.Professions, "Id", "Name");
                return View(form);
            }

            var info = new Info();
            form.Fill(info);

            info.Profession = _siteContext.Professions.FirstOrDefault(x => x.Id == form.ProfessionId);
            info.Skills = form.Skills;

            if (form.Images != null)
            {
                info.Images = new List<ImageFile>();
                foreach (var formImage in form.Images)
                {
                    info.Images.Add(await _localUploadedFileStorage.SaveUploadedFileAsync(formImage));
                }
            }

            _siteContext.Infos.Add(info);
            await _siteContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _siteContext.Infos.FirstAsync(x => x.Id == id);

            if (model.Images != null)
            {
                foreach (var image in model.Images)
                {
                    _localUploadedFileStorage.DeleteUploadedFile(image);
                }
            }

            await _siteContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _siteContext
                .Infos
                .Include(p => p.Profession)
                .Include(i => i.Images)
                .Include(s => s.Skills)
                .ThenInclude(s => s.Skill)
                .ThenInclude(i => i.Image)
                .FirstAsync(x => x.Id == id);
            ViewBag.Professions = new SelectList(await _siteContext.Professions.ToListAsync(), "Id", "Name");
            ViewData["Skills"] = model.Skills;
            ViewData["Images"] = model.Images;

            var form = new InfoForm(model);
            return View(form);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] InfoForm form)
        {
            var model = await _siteContext
                .Infos
                .Include(p => p.Profession)
                .Include(i => i.Images)
                .Include(s => s.Skills)
                .ThenInclude(s => s.Skill)
                .ThenInclude(i => i.Image)
                .FirstAsync(x => x.Id == id);

            if (!ModelState.IsValid)
            {
                ViewBag.Professions = new SelectList(await _siteContext.Professions.ToListAsync(), "Id", "Name");
                ViewData["Images"] = model.Images;
                ViewData["InfoSkills"] = model.Skills;
                return View(form);
            }


            form.Fill(model);
            if (model.Profession.Id != form.ProfessionId)
            {
                model.Profession = await _siteContext.Professions.FirstAsync(x => x.Id == form.ProfessionId);
            }

            if (form.Images != null)
            {
                foreach (var formImage in form.Images)
                {
                    model.Images.Add(await _localUploadedFileStorage.SaveUploadedFileAsync(formImage));
                }
            }

            await _siteContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Show(int id)
        {
            return View(await
                _siteContext
                .Infos
                .Include(p => p.Profession)
                .Include(i => i.Images)
                .Include(s => s.Skills)
                .ThenInclude(s => s.Skill)
                .ThenInclude(i => i.Image)
                .FirstAsync(x => x.Id == id)
                );
        }

        [HttpPost]
        public async Task<IActionResult> DeleteImage([FromBody] DeleteInfoImageForm form)
        {
            try
            {
                var image = await _siteContext.ImageFiles.FirstAsync(x => x.Id == form.ImageId);
                _localUploadedFileStorage.DeleteUploadedFile(image);
                _siteContext.Remove(image);
                await _siteContext.SaveChangesAsync();
                return Json(new { Ok = true });
            }
            catch (Exception)
            {
                return Json(new { Ok = false });
            }
        }
    }

}
