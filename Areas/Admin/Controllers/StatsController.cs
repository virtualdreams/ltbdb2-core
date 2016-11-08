using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ltbdb.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize]
	//[LogError(Order = 0)]
	//[HandleError(View = "Error", Order = 99)]
	public class StatsController : Controller
	{
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}
	}
}
