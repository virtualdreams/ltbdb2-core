using AutoMapper;
using LtbDb.Models;
using LtbDb.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Security.Claims;
using System;

namespace LtbDb.Controllers
{
	public class AccountController : Controller
	{
		private readonly IMapper Mapper;
		private readonly AppSettings AppSettings;

		public AccountController(IMapper mapper, IOptionsSnapshot<AppSettings> settings)
		{
			Mapper = mapper;
			AppSettings = settings.Value;
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

			if (AppSettings.Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase) && AppSettings.Password.Equals(model.Password))
			{
				var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, model.Username, ClaimValueTypes.String),
						new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String)
					};

				var _identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
				var _principal = new ClaimsPrincipal(_identity);

				HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, _principal,
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
			HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

			return RedirectToAction("index", "home");
		}
	}
}
