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
        private readonly LocalUploadedFileStorage _localUploadedFileStorage;

        public InfoController(IObjectCollectionStorage<List<Info>> infoStorage, IObjectCollectionStorage<List<Profession>> professionStorage, IObjectCollectionStorage<List<Skill>> skillStorage, LocalUploadedFileStorage localUploadedFileStorage)
        {
            _infoStorage = infoStorage;
            _professionStorage = professionStorage;
            _skillStorage = skillStorage;
            _localUploadedFileStorage = localUploadedFileStorage;
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

            if (form.Images != null)
            {
                info.Images = new List<ImageFile>();
                foreach (var formImage in form.Images)
                {
                    info.Images.Add(await _localUploadedFileStorage.SaveUploadedFileAsync(formImage));
                }
            }

            _infoStorage.items.Add(info);
            await _infoStorage.SaveAsync();
            return RedirectToAction("Index");
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var model = _infoStorage.items.FirstOrDefault(x => x.Id == id);
            _infoStorage.items.Remove(model);

            if (model.Images != null)
            {
                foreach (var image in model.Images)
                {
                    _localUploadedFileStorage.DeleteUploadedFile(image);
                }
            }

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
            ViewData["Images"] = info.Images;

            var form = new InfoForm(info);
            return View(form);
        }

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] InfoForm form)
        {
            var info = _infoStorage.items.First(x => x.Id == id);
            if (!ModelState.IsValid)
            {
                ViewBag.Professions = new SelectList(_professionStorage.items, "Id", "Name", form.ProfessionId);
                ViewData["Images"] = info.Images;
                ViewData["InfoSkills"] = info.Skills;
                return View(form);
            }


            form.Fill(info);

            if (form.Images != null)
            {
                if (info.Images == null)
                {
                    info.Images = new List<ImageFile>();
                }
                foreach (var formImage in form.Images)
                {
                    info.Images.Add(await _localUploadedFileStorage.SaveUploadedFileAsync(formImage));
                }
            }

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

        [HttpPost]
        public async Task<IActionResult> DeleteImage([FromBody] DeleteInfoImageForm form)
        {
            try
            {
                var model = _infoStorage.items.First(x => x.Id == form.InfoId);
                var image = model.Images.First(x => x.Id == form.ImageId);

                _localUploadedFileStorage.DeleteUploadedFile(image);
                model.Images.Remove(image);
                await _infoStorage.SaveAsync();
                return Json(new { Ok = true });
            }
            catch (Exception)
            {
                return Json(new { Ok = false });
            }
        }

    }
}
