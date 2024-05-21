using ASP_NET_PZ_03.Models;
using ASP_NET_PZ_03.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP_NET_PZ_03.Controllers
{
	public class SkillController : Controller
	{
		private readonly IObjectCollectionStorage<List<Skill>> _skillStorage;

		public SkillController(IObjectCollectionStorage<List<Skill>> skillstorage)
		{
			_skillStorage= skillstorage;
		}

		public IActionResult Index()
		{
			return View(_skillStorage.items);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View(new Skill());
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromForm] Skill form)
		{
			if (!ModelState.IsValid)
			{
				return View(form);
			}
			var Random = new Random();
			var Skill = new Skill();
			Skill.Id = Random.Next(1,1000);
			Skill.Name = form.Name;

			_skillStorage.items.Add(Skill);
			await _skillStorage.SaveAsync();

			return RedirectToAction("Index");
		}

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = _skillStorage.items.FirstOrDefault(x => x.Id == id);
            _skillStorage.items.Remove(model);
            await _skillStorage.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public IActionResult Edit(int id)
        {
            var skill = _skillStorage.items.FirstOrDefault(x => x.Id == id);
            if (skill == null)
            {
                return NotFound();
            }
            ViewBag.Skills = new SelectList(_skillStorage.items, "Id", "Name", skill.Id);
            return View(skill);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] Skill form)
        {
            if (!ModelState.IsValid)
            {
                ViewBag.Skills = new SelectList(_skillStorage.items, "Id", "Name", form.Id);
                return View(form);
            }

            var skill = _skillStorage.items.First(x => x.Id == id);
            skill.Name = form.Name;

            await _skillStorage.SaveAsync();
            return RedirectToAction("Index");
        }
    }
}
