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
		private readonly BookService Book;
		private readonly CategoryService Category;
		private readonly TagService Tag;
		private readonly MaintenanceService Maintenance;

		public HomeController(BookService book, CategoryService category, TagService tag, MaintenanceService maintenance)
		{
			Book = book;
			Category = category;
			Tag = tag;
			Maintenance = maintenance;
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

			return Json(Maintenance.Export(), new JsonSerializerSettings { Formatting = Formatting.Indented });
		}

		[HttpGet]
		public IActionResult Stats()
		{
			return Json(Maintenance.Stats(), new JsonSerializerSettings { Formatting = Formatting.Indented });
		}
	}
}
