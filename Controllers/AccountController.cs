using System;
using System.Collections.Generic;
using System.Security.Claims;
using AutoMapper;
using ltbdb.Core.Helpers;
using ltbdb.Core.Services;
using ltbdb.Models;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace ltbdb.Controllers
{
    public class AccountController : Controller
    {
		private readonly IMapper Mapper;
		private readonly ILogger<AccountController> Log;
		private readonly UserService Authentication;

		public AccountController(IMapper mapper, UserService authentication, ILogger<AccountController> logger)
		{
			Mapper = mapper;
			Authentication = authentication;
			Log = logger;
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
			
			if(GlobalConfig.Get().UseDatabaseAuthentication)
			{
				var _user = Authentication.GetUser(model.Username, model.Password);
				if(_user != null && _user.Enabled)
				{
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, _user.Username, ClaimValueTypes.String),
						new Claim(ClaimTypes.Role, _user.Role, ClaimValueTypes.String)
					};

					var _identity = new ClaimsIdentity(claims, "local");
					var _principal = new ClaimsPrincipal(_identity);

					HttpContext.Authentication.SignInAsync("ltbdb", _principal, 
						new AuthenticationProperties {
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
			}
			else
			{
				if (GlobalConfig.Get().Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase) && GlobalConfig.Get().Password.Equals(model.Password))
				{
					var claims = new List<Claim>
					{
						new Claim(ClaimTypes.Name, model.Username, ClaimValueTypes.String),
						new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String)
					};

					var _identity = new ClaimsIdentity(claims, "local");
					var _principal = new ClaimsPrincipal(_identity);

					HttpContext.Authentication.SignInAsync("ltbdb", _principal, 
						new AuthenticationProperties {
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
			HttpContext.Authentication.SignOutAsync("ltbdb");

			return RedirectToAction("index", "home");
		}
    }
}
