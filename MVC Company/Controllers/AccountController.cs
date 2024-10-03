using Company.Data.Models;
using Company.Services.AccountDtos;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace MVC_Company.Controllers
{
	public class AccountController : Controller
	{
		private readonly UserManager<ApplicationUser> _userManager;

		public AccountController(UserManager<ApplicationUser> userManager)
        {
			_userManager = userManager;
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
					IsAgree = model.IsAgree,
				};
				var result = await _userManager.CreateAsync(User , model.Password);
				if (result.Succeeded)
					RedirectToAction("Login");

				
			}
			ModelState.AddModelError(string.Empty, "Error");
			return View(model);
		}
	}
}
