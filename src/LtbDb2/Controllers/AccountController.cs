using AutoMapper;
using FluentValidation;
using LtbDb.Core.Interfaces;
using LtbDb.FluentValidation;
using LtbDb.Models;
using LtbDb.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;

namespace LtbDb.Controllers
{
	public class AccountController : Controller
	{
		private readonly ILogger<AccountController> Log;

		private readonly IMapper Mapper;

		private readonly AppSettings AppSettings;

		private readonly IUserService UserService;

		private readonly IValidator<LoginModel> LoginModelValidator;

		public AccountController(
			ILogger<AccountController> log,
			IMapper mapper,
			IOptionsSnapshot<AppSettings> settings,
			IUserService user,
			IValidator<LoginModel> loginModelValidator)
		{
			Log = log;
			Mapper = mapper;
			AppSettings = settings.Value;
			UserService = user;
			LoginModelValidator = loginModelValidator;
		}

		[HttpGet]
		public IActionResult Login()
		{
			var login = new LoginModel();

			return View(login);
		}

		[HttpPost]
		public async Task<IActionResult> Login(LoginModel model, string returnUrl)
		{
			var _result = await LoginModelValidator.ValidateAsync(model);
			if (!_result.IsValid)
			{
				_result.AddToModelState(ModelState);
				return View("Login", model);
			}

			if (UserService.Login(model.Username, model.Password))
			{
				var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, model.Username, ClaimValueTypes.String),
						new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String)
					};

				var _identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var _principal = new ClaimsPrincipal(_identity);

				await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, _principal,
					new AuthenticationProperties
					{
						IsPersistent = model.Remember,
						AllowRefresh = model.Remember
					}
				);
			}
			else
			{
				ModelState.AddModelError("error", "Benutzername oder Passwort falsch.");
				return View("Login", model);
			}

			// return to target page.
			if (Url.IsLocalUrl(returnUrl))
			{
				return Redirect(returnUrl);
			}
			else
			{
				return RedirectToAction("index", "home");
			}
		}

		[HttpGet]
		public IActionResult Logout()
		{
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return RedirectToAction("index", "home");
		}
	}
}
