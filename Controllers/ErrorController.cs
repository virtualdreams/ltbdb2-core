using Microsoft.AspNetCore.Mvc;

namespace ltbdb.Controllers
{
    public class ErrorController : Controller
    {
        public IActionResult Index()
        {
			return View();
        }

		public IActionResult Http404()
		{
			return View();
		}
    }
}
