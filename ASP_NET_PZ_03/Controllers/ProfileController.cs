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

        private async Task<User> GetCurrentUserAsync()
        {
            var currentUserId = User.Claims.First(x => x.Type == ClaimTypes.NameIdentifier).Value;
            return await _userManager.Users.FirstAsync(x => x.Id.ToString() == currentUserId);
        }

        public async Task<IActionResult> Index()
        {
            return View(await GetCurrentUserAsync());
        }

        [HttpGet]
        public async Task<IActionResult> ResetPasswordModalAjax()
        {
            ViewData["User"] = await GetCurrentUserAsync();
            return PartialView("ChangePasswordModalAjax",new ChangePasswordForm());
        }

        [HttpPost]
        public async Task<IActionResult> ResetPasswordModalAjax([FromForm] ChangePasswordForm form)
        {
            var user = await GetCurrentUserAsync();
            ViewData["User"] = user;
            if (!ModelState.IsValid)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("ChangePasswordModalAjax",form);
            }

            if (!await _userManager.CheckPasswordAsync(user,form.Password))
            {
                ModelState.AddModelError(nameof(form.Password),"Incorrect Password");
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                return PartialView("ChangePasswordModalAjax", form);
            }

            if (form.NewPassword != form.ConfirmPassword)
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
                ModelState.AddModelError(nameof(form.ConfirmPassword), "Password mismatch");
                return PartialView("ChangePasswordModalAjax", form);
            }

            var token = await _userManager.GeneratePasswordResetTokenAsync(user);
            var result = await _userManager.ResetPasswordAsync(user, token, form.NewPassword);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(nameof(form.NewPassword), String.Join(";", result.Errors.Select(x => x.Description)));
                return PartialView("ChangePasswordModalAjax", form);
            }

            return RedirectToAction("Index","Profile");
        }

        [HttpGet]
        public async Task<IActionResult> ChangeProfileAjax()
        {
            ViewData["User"] = await GetCurrentUserAsync();
            return PartialView("ChangeProfileModalAjax", new ChangeProfileForm());
        }

        public IActionResult CloseAction()
        {
            return RedirectToAction("Index");
        }

        [HttpPost]
        public async Task<IActionResult> SubmitChanges([FromForm] ChangeProfileForm form)
        {
            var user = await GetCurrentUserAsync();

            if (!string.IsNullOrEmpty(form.Email) && form.Email != user.Email)
            {
                var token = await _userManager.GenerateChangeEmailTokenAsync(user, form.Email);
                var result = await _userManager.ChangeEmailAsync(user, form.Email, token);

                if (!result.Succeeded)
                {
                    ModelState.AddModelError(nameof(form.Email), "The email change failed");
                    return View(form);
                }
            }

            if (!string.IsNullOrEmpty(form.FullName) && form.FullName != user.FullName)
            {
                user.FullName = form.FullName;
                var updateResult = await _userManager.UpdateAsync(user);

                if (!updateResult.Succeeded)
                {
                    ModelState.AddModelError(nameof(form.FullName), "Failed to update full name");
                    return View(form);
                }

                var claimsIdentity = (ClaimsIdentity)User.Identity;
                var existingFullNameClaim = claimsIdentity.FindFirst("FullName");
                if (existingFullNameClaim != null)
                {
                    claimsIdentity.RemoveClaim(existingFullNameClaim);
                }
                claimsIdentity.AddClaim(new Claim("FullName", user.FullName));

                await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
                await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme,
                    new ClaimsPrincipal(claimsIdentity));
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }





    }
}