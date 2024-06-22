using Microsoft.AspNetCore.Mvc;
using ASP_NET_PZ_03.Models.Forms;
using ASP_NET_PZ_03.Models;
using Microsoft.EntityFrameworkCore;

namespace ASP_NET_PZ_03.Controllers
{
    public class ReviewController : Controller
    {
        private readonly SiteContext _siteContext;

        public ReviewController(SiteContext siteContext)
        {
            _siteContext = siteContext;
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public async Task<IActionResult> ShowReviewsModalAjax(int id)
        {
            var model = await _siteContext
                .Infos
                .Include(p => p.Profession)
                .Include(r => r.Reviews)
                .FirstAsync(x => x.Id == id);

            return PartialView("~/Views/Home/ShowReviewsModalAjax.cshtml",model);
        }


        [HttpPost]
        public async Task<IActionResult> AddReview(int id, [FromBody] ReviewForm form)
        {
            var model = await _siteContext.Infos.FirstAsync(x => x.Id == id);

            var review = new Review
            {
                ClientEmail = form.ClientEmail,
                ClientName = form.ClientName,
                CreatedAt = form.CreatedAt ?? DateTime.Now,
                Rate = form.Rate,
                Text = form.Text,
            };

            model.Reviews.Add(review);
            await _siteContext.SaveChangesAsync();

            return Json(new { Ok = true });
        }
    }
}
