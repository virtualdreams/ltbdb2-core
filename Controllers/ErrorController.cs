using Microsoft.AspNetCore.Mvc;

namespace ltbdb.Controllers
{
    //[LogError(Order = 0)]
    //[HandleError(View = "Error", Order=99)]
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
