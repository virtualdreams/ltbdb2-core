using AutoMapper;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Security.Claims;
using ltbdb.Models;

namespace ltbdb.Controllers
{
	public class AccountController : Controller
	{
		private readonly IMapper Mapper;
		private readonly ILogger<AccountController> Log;
		private readonly Settings Options;

		public AccountController(IMapper mapper, IOptionsSnapshot<Settings> settings, ILogger<AccountController> log)
		{
			Mapper = mapper;
			Log = log;
			Options = settings.Value;
		}

		[HttpGet]
		public IActionResult Login()
		{
			var login = new LoginModel();

			return View(login);
		}

		[HttpPost]
		public IActionResult Login(LoginModel model, string returnUrl)
		{
			if (!ModelState.IsValid)
			{
				return View("Login", model);
			}

			if (Options.Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase) && Options.Password.Equals(model.Password))
			{
				var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, model.Username, ClaimValueTypes.String),
						new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String)
					};

				var _identity = new ClaimsIdentity(claims, "local");
				var _principal = new ClaimsPrincipal(_identity);

				AuthenticationHttpContextExtensions.SignInAsync(HttpContext, _principal,
					new AuthenticationProperties
					{
						IsPersistent = true,
						AllowRefresh = true
					}
				).Wait();
			}
			else
			{
				ModelState.AddModelError("failed", "Benutzername oder Passwort falsch.");
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
			AuthenticationHttpContextExtensions.SignOutAsync(HttpContext);

			return RedirectToAction("index", "home");
		}
	}
}
