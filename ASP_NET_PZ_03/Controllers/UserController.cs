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
    public class UserController : Controller
    {
        private readonly UserManager<User> _userManager;
        public UserController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _userManager.Users.ToListAsync());
        }

        public async Task<IActionResult> DeleteAjax(int id)
        {
            try
            {
                var model = await _userManager.FindByIdAsync(id.ToString());
                if (model == null)
                {
                    throw new Exception("Error.Not Found");
                }

                var result = await _userManager.DeleteAsync(model);

                return Json(new { Ok = result.Succeeded });
            }
            catch (Exception)
            {
                return Json(new { Ok = false }); 
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View(new RegisterForm());
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromForm] RegisterForm form)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            if (form.Password != form.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(form.ConfirmPassword), "Password mismatch");
                return View(form);
            }

            if (null != await _userManager.FindByEmailAsync(form.Email))
            {
                ModelState.AddModelError(nameof(form.Email), "User with " + form.Email + " already exists");
                return View(form);
            }

            var user = new User
            {
                Email = form.Email,
                UserName = form.Email,
                FullName = form.FullName,
            };

            var result = await _userManager.CreateAsync(user,form.Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(nameof(form.Password), String.Join(";", result.Errors.Select(x => x.Description)));
                return View(form);
            }

            return RedirectToAction("Index","User");
        }

        [HttpGet]
        public async Task<IActionResult> ResetPassword(int id)
        {
            ViewData["User"] = await _userManager.FindByIdAsync(id.ToString());
            return View(new ResetPasswordForm());
        }

        [HttpPost]
        public async Task<IActionResult> ResetPassword(int id, [FromForm] ResetPasswordForm form)
        {
            var user = await _userManager.FindByIdAsync(id.ToString());
            ViewData["User"] = user;
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            if (form.NewPassword != form.ConfirmPassword)
            {
                ModelState.AddModelError(nameof(form.ConfirmPassword),"Password mismatch");
                return View(form);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user,token,form.NewPassword);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(nameof(form.NewPassword), String.Join(";", result.Errors.Select(x => x.Description)));
                return View(form);
            }

            return RedirectToAction("Index","User");
        }
    }
}
