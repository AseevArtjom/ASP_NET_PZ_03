using Microsoft.AspNetCore.Mvc;
using ASP_NET_PZ_03.Models.Forms;
using ASP_NET_PZ_03.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging.Console;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ASP_NET_PZ_03.Controllers
{
    public class ProfileController : Controller
    {
        private readonly UserManager<User> _userManager;
        public ProfileController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            var currentUserId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            var me = await _userManager
                .Users
                .FirstAsync(x => x.Id.ToString() == currentUserId);
            return View(me);
        }

    }
}