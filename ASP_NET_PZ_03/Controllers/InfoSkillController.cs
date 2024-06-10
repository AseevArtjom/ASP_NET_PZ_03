using ASP_NET_PZ_03.Models;
using ASP_NET_PZ_03.Models.Forms;
using ASP_NET_PZ_03.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Linq;

namespace ASP_NET_PZ_03.Controllers
{
    public class InfoSkillController : Controller
    {
        private readonly SiteContext _siteContext;

        public InfoSkillController(SiteContext siteContext)
        {
            _siteContext = siteContext;
        }

        public async Task<IActionResult> Index()
        {
            return View(_siteContext.InfoSkills);
        }

        [HttpGet]
        public async Task<IActionResult> Create(int id)
        {
            var info = await _siteContext.Infos
                .Include(s => s.Skills)
                .ThenInclude(s => s.Skill)
                .FirstAsync(x => x.Id == id);

            var addedSkills = info.Skills.Select(s => s.Skill.Id);

            ViewData["Skills"] = await _siteContext.Skills
                .Where(skill => !addedSkills.Contains(skill.Id))
                .ToListAsync();

            return View(new InfoSkillForm());
        }



        [HttpPost]
        public async Task<IActionResult> Create(int id, [FromForm] InfoSkillForm form)
        {
            var info = await _siteContext.Infos
                .Include(s => s.Skills)
                .ThenInclude(s => s.Skill)
                .FirstAsync(x => x.Id == id);

            if (info == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                var addedSkills = info.Skills.Select(s => s.Skill.Id);

                ViewData["Skills"] = await _siteContext.Skills
                    .Where(skill => !addedSkills.Contains(skill.Id))
                    .ToListAsync();

                return View(form);
            }

            var skill = await _siteContext.Skills.FirstAsync(x => x.Id == form.SkillId);
            if (skill == null)
            {
                ModelState.AddModelError("SkillId", "Invalid skill ID.");
                ViewBag.Skills = _siteContext.Skills;
                return View(form);
            }

            var infoSkill = new InfoSkill
            {
                Skill = skill,
                Level = form.Level
            };

            if (info.Skills == null)
            {
                info.Skills = new List<InfoSkill>();
            }
            info.Skills.Add(infoSkill);

            await _siteContext.SaveChangesAsync();

            return RedirectToAction("Edit", "Info", new { id = id });
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = await _siteContext.InfoSkills.FirstAsync(x => x.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            _siteContext.InfoSkills.Remove(model);
            _siteContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int infoId,int infoSkillId)
        {
            var info = await _siteContext
                .Infos
                .Include(s => s.Skills)
                .ThenInclude(s => s.Skill)
                .FirstAsync(x => x.Id == infoId);
            var model = await _siteContext.InfoSkills.FirstAsync(x => x.Id == infoSkillId);

            var addedSkills = info.Skills.Select(s => s.Skill.Id);

            ViewData["Skills"] = await _siteContext.Skills
                .Where(skill => !addedSkills.Contains(skill.Id))
                .ToListAsync();

            if (model == null)
            {
                return NotFound();
            }
            return View(new InfoSkillForm()
            {
                Level = model.Level,
                SkillId = model.Skill.Id
            });
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] Skill form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }
            
            var model = await _siteContext.Skills.FirstAsync(x => x.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            model.Title = form.Title;

            await _siteContext.SaveChangesAsync();
            return RedirectToAction("Index");
        }
    }
}
