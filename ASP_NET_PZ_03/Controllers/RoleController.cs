using Microsoft.AspNetCore.Mvc;
using ASP_NET_PZ_03.Models.Forms;
using ASP_NET_PZ_03.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging.Console;
using Microsoft.EntityFrameworkCore;
using ASP_NET_PZ_03.Migrations;
using User = ASP_NET_PZ_03.Models.User;

namespace ASP_NET_PZ_03.Controllers
{
    public class RoleController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RoleController(UserManager<User> userManager, RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> UserRoles(int id)
        {
            var roles = await _roleManager.Roles.ToListAsync();

            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            var userRoles = await _userManager.GetRolesAsync(user);

            ViewData["User"] = user;
            ViewData["Roles"] = roles;

            return View(new UserRolesForm
            {
                Roles = roles.Select(x => new UserRole
                {
                    Name = x.Name,
                    Enable = userRoles.Contains(x.Name)
                }).ToList()
            });
        }

        [HttpPost]
        public async Task<IActionResult> UserRoles(int id, [FromForm] UserRolesForm form)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            if (user == null)
            {
                return NotFound();
            }
            ViewData["User"] = user;

            var userRoles = await _userManager.GetRolesAsync(user);

            form.Roles.ForEach(async x =>
            {
                if (x.Enable)
                {
                    await _userManager.AddToRoleAsync(user, x.Name);
                }
                else
                {
                    await _userManager.RemoveFromRoleAsync(user, x.Name);
                }
            });

            await _userManager.UpdateAsync(user);

            return RedirectToAction("Index", "User");
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }
    }
}
