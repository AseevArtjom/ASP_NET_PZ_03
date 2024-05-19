using ASP_NET_PZ_03.Models;
using ASP_NET_PZ_03.Services;
using Microsoft.AspNetCore.Mvc;

namespace ASP_NET_PZ_03.Controllers
{
	public class InfoController : Controller
	{
		private readonly IObjectCollectionStorage<List<Info>> _fileStorage;


		public InfoController(IObjectCollectionStorage<List<Info>> storage)
		{
			_fileStorage = storage;
		}

		public IActionResult Index()
		{
			return View(_fileStorage.items);
		}

		[HttpGet]
		public IActionResult Create()
		{
			return View(new Info());
		}

		[HttpPost]
		public async Task<IActionResult> Create([FromForm] Info form)
		{
			if (!ModelState.IsValid)
			{
				return View(form);
			}

			var info = new Info(); 
			var random = new Random();
            info.Id = random.Next(1, 1000);
            info.FirstName = form.FirstName;
			info.LastName = form.LastName;
			info.Age = form.Age;
			info.City = form.City;
			info.Description = form.Description;
			info.Busy = form.Busy;
			info.BirthDay = form.BirthDay;

			_fileStorage.items.Add(info);
			await _fileStorage.SaveAsync();
			return RedirectToAction("Index");
		}


		

		[HttpGet]
		public async Task<IActionResult> Delete(int id)
		{
			var model = _fileStorage.items.FirstOrDefault(x => x.Id == id);
			_fileStorage.items.Remove(model);
			await _fileStorage.SaveAsync();
			return RedirectToAction("Index");
		}

        [HttpGet]
        public IActionResult Edit(int id)
		{
			return View(_fileStorage.items.FirstOrDefault(x => x.Id == id));
		}

        [HttpPost]
        public async Task<IActionResult> Edit(int id, [FromForm] Info form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

			var info = _fileStorage.items.First(x => x.Id == id);

            info.FirstName = form.FirstName;
            info.LastName = form.LastName;
            info.City = form.City;
            info.Description = form.Description;
            info.Age = form.Age;
            info.Busy = form.Busy;
            info.BirthDay = form.BirthDay;

            await _fileStorage.SaveAsync();

            return RedirectToAction("Index");
        }

        public IActionResult Show(int id)
		{
			return View(_fileStorage.items.First(x => x.Id == id));
		}
	}
}
