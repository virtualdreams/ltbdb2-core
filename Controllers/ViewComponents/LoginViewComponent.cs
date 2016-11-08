using System.Linq;
using ltbdb.Core.Services;
using ltbdb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ltbdb.ViewComponents
{
	public class LoginViewComponent : ViewComponent
    {
		public IViewComponentResult Invoke()
		{
			return View();
		}
	}
}