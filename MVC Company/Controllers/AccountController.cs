using Company.Data.Models;
using Company.Services.AccountDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MVC_Company.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;

		public AccountController(UserManager<ApplicationUser> userManager , SignInManager<ApplicationUser> signInManager)
        {
			_userManager = userManager;
			_signInManager = signInManager;
		}
        public IActionResult Register()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Register(RegisterDto model)
		{
			if (ModelState.IsValid)
			{
				var User = new ApplicationUser()
				{
					UserName = model.Email.Split('@')[0],
					Email = model.Email,
					FirstName = model.FirstName,
					LastName = model.LastName,
					IsAgree = model.IsAgree
				};
				var result = await _userManager.CreateAsync(User, model.Password);
				if (result.Succeeded)
					return RedirectToAction(nameof(Login));
				else
				foreach (var error in result.Errors)
				  ModelState.AddModelError(string.Empty, error.Description);
		    }

				return View(model);
		}
		public IActionResult Login()
		{
			return View();
		}
		[HttpPost]
		public async Task<IActionResult> Login(LoginDto model)
		{
			if (ModelState.IsValid)
			{
				var User = await _userManager.FindByEmailAsync(model.Email);
				if (User is not null)
				{
					var flag = await _userManager.CheckPasswordAsync(User, model.Password);
					if (flag)
					{
						var result = await _signInManager.PasswordSignInAsync(User, model.Password, model.RememberMe, false);
						if (result.Succeeded)
						{
							return RedirectToAction("Index", "Home");
						}
					}
					else
						{
							ModelState.AddModelError(string.Empty, "ErrorPassword");
						}
				}
				else
				{
					ModelState.AddModelError(string.Empty, "UserNotExist");
				}

			}

			return View();
		}

	}
}
