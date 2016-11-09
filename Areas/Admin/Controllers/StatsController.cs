using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace ltbdb.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Policy = "AdministratorOnly")]
	public class StatsController : Controller
	{
		[HttpGet]
		public ActionResult Index()
		{
			return View();
		}
	}
}
