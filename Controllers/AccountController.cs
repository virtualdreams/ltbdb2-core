using System.Collections.Generic;
using System.Security.Claims;
using ltbdb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ltbdb.Controllers
{
    //[LogError(Order = 0)]
    //[HandleError(View = "Error", Order = 99)]
    public class AccountController : Controller
    {
		//private static readonly ILog Log = LogManager.GetLogger(typeof(AccountController));

        [HttpGet]
        public IActionResult Index()
        {
            return View();
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
			
			var claims = new List<Claim>{
				new Claim(ClaimTypes.Name, "TEST", ClaimValueTypes.String)
			};


			var user = new ClaimsIdentity(claims, "local");
			var principal = new ClaimsPrincipal(user);

			HttpContext.Authentication.SignInAsync("ltbdb", principal).Wait();

			// check login credentials
			//if (GlobalConfig.Get().Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase) && GlobalConfig.Get().Password.Equals(model.Password))
			//{
			//	FormsAuthentication.SetAuthCookie(model.Username, false);
			//}
			//else
			//{
			//	ModelState.AddModelError("failed", "Benutzername oder Passwort falsch.");
			//	return View("login", model);
			//}

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
			//FormsAuthentication.SignOut();

			return RedirectToAction("index", "home");
		}
    }
}
