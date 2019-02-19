using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System;
using ltbdb.Core.Services;

namespace ltbdb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Policy = "AdministratorOnly")]
	public class HomeController : Controller
	{
		private readonly BookService BookService;
		private readonly CategoryService CategoryService;
		private readonly TagService TagService;
		private readonly MaintenanceService MaintenanceService;

		public HomeController(BookService book, CategoryService category, TagService tag, MaintenanceService maintenance)
		{
			BookService = book;
			CategoryService = category;
			TagService = tag;
			MaintenanceService = maintenance;
		}

		[HttpGet]
		public IActionResult Index()
		{
			return View();
		}

		[HttpGet]
		public IActionResult Export()
		{
			// add header to force it as download
			Response.Headers.Add("Content-Disposition", $"attachment; filename=ltbdb-export-{DateTime.Now.ToString("yyyyMMddHHmmss")}.json");

			return Json(MaintenanceService.Export(), new JsonSerializerSettings { Formatting = Formatting.Indented });
		}

		[HttpGet]
		public IActionResult Stats()
		{
			return Json(MaintenanceService.Stats(), new JsonSerializerSettings { Formatting = Formatting.Indented });
		}
	}
}
