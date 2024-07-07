using Microsoft.AspNetCore.Mvc;
using ASP_NET_PZ_03.Models.Forms;
using ASP_NET_PZ_03.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging.Console;
using Microsoft.EntityFrameworkCore;
using ASP_NET_PZ_03.Migrations;
using User = ASP_NET_PZ_03.Migrations.User;

namespace ASP_NET_PZ_03.Controllers
{
    public class RoleControlle : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly RoleManager<IdentityRole<int>> _roleManager;

        public RoleControlle(UserManager<User> userManager,RoleManager<IdentityRole<int>> roleManager)
        {
            _userManager = userManager;
            _roleManager = roleManager;
        }

        [HttpGet]
        public async Task<IActionResult> User(int id)
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

            return View(new UserRolesForm {
                Roles = roles.Select(x => new UserRole
                { 
                    Name = x.Name,
                    Enable = userRoles.Contains(x.Name)
                }).ToList()
            });
        }
    }
}