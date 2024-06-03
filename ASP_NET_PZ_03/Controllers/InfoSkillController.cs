using ASP_NET_PZ_03.Models;
using ASP_NET_PZ_03.Models.Forms;
using ASP_NET_PZ_03.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP_NET_PZ_03.Controllers
{
    public class InfoSkillController : Controller
    {
        private readonly IObjectCollectionStorage<List<Info>> _infoStorage;
        private readonly IObjectCollectionStorage<List<Skill>> _skillStorage;

        public InfoSkillController(IObjectCollectionStorage<List<Skill>> storage, IObjectCollectionStorage<List<Info>> infoStorage)
        {
            _skillStorage = storage;
            _infoStorage = infoStorage;
        }

        public IActionResult Index()
        {
            return View(_skillStorage.items);
        }

        [HttpGet]
        public IActionResult Create(int id)
        {
            var info = _infoStorage.items.FirstOrDefault(x => x.Id == id);
            if (info == null)
            {
                return NotFound();
            }
            var addedSkills = info.Skills?.Select(s => s.Skill.Id).ToList() ?? new List<int>();
            ViewData["Skills"] = (_skillStorage.items.Where(x => !addedSkills.Contains(x.Id))).ToList();
            return View(new InfoSkillForm());
        }

        [HttpPost]
        public async Task<IActionResult> Create(int id, [FromForm] InfoSkillForm form)
        {
            var info = _infoStorage.items.FirstOrDefault(x => x.Id == id);
            if (info == null)
            {
                return NotFound();
            }
            if (!ModelState.IsValid)
            {
                ViewBag.Skills = _skillStorage.items;
                return View(form);
            }

            var skill = _skillStorage.items.FirstOrDefault(x => x.Id == form.SkillId);
            if (skill == null)
            {
                ModelState.AddModelError("SkillId", "Invalid skill ID.");
                ViewBag.Skills = _skillStorage.items;
                return View(form);
            }

            var infoSkill = new InfoSkill
            {
                Id = new Random().Next(1, 1000),
                Skill = skill,
                Level = form.Level
            };

            if (info.Skills == null)
            {
                info.Skills = new List<InfoSkill>();
            }
            info.Skills.Add(infoSkill);

            await _infoStorage.SaveAsync();

            return RedirectToAction("Edit", "Info", new { id = id });
        }

        [HttpGet]
        public IActionResult Delete(int id)
        {
            var model = _skillStorage.items.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            _skillStorage.items.Remove(model);
            _skillStorage.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int infoId,int infoSkillId)
        {
            var info = _infoStorage.items.First(x => x.Id == infoId);
            var model = info.Skills.First(x => x.Id == infoSkillId);

            var addedSkills = info.Skills?.Select(s => s.Skill.Id).ToList() ?? new List<int>();
            ViewData["Skills"] = _skillStorage.items.Where(x=>x.Id == model.Skill.Id || !addedSkills.Contains(x.Id)).ToList();

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
            
            var model = _skillStorage.items.FirstOrDefault(x => x.Id == id);
            if (model == null)
            {
                return NotFound();
            }
            model.Title = form.Title;

            await _skillStorage.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
