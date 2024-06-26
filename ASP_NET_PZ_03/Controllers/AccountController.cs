using Microsoft.AspNetCore.Mvc;
using ASP_NET_PZ_03.Models.Forms;
using ASP_NET_PZ_03.Models;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging.Console;

namespace ASP_NET_PZ_03.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        public AccountController(UserManager<User> userManager)
        {
            _userManager = userManager;
        }

        [HttpGet]
        public IActionResult Login(string? returnPath)
        {
            return View(new LoginForm());
        }

        [HttpGet]
        public IActionResult Register(string? returnPath)
        {
            return View(new RegisterForm());
        }


        [HttpPost]
        public async Task<IActionResult> Register([FromForm] RegisterForm form, string? returnPath)
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
            };

            var result = await _userManager.CreateAsync(user,form.Password);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(nameof(form.Password), String.Join(";", result.Errors.Select(x => x.Description)));
                return View(form);
            }

            await SignIn(user);

            if (returnPath != null)
            {
                return Redirect(returnPath);
            }
            return RedirectToAction("Index", "Home");
        }
        
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginForm form, string? returnPath)
        {
            if (!ModelState.IsValid)
            {
                return View(form);
            }

            var user = await _userManager.FindByEmailAsync(form.Email);

            if (user == null)
            {
                ModelState.AddModelError(nameof(form.Email),"User with this email not found");
                return View(form);
            }

            if (await _userManager.CheckPasswordAsync(user,form.Password))
            {
                ModelState.AddModelError(nameof(form.Password), "Invalid password");
                return View(form);
            }

            await SignIn(user);

            if (returnPath != null)
            {
                return Redirect(returnPath);
            }

            return View();
        }

        private async Task SignIn(User user)
        {
            var identity = new ClaimsIdentity(IdentityConstants.ApplicationScheme);

            identity.AddClaim(new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()));
            identity.AddClaim(new Claim(ClaimTypes.Email, user.Email));

            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(IdentityConstants.ApplicationScheme, principal);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(IdentityConstants.ApplicationScheme);
            return RedirectToAction("Index", "Home");
        }
    }
}
