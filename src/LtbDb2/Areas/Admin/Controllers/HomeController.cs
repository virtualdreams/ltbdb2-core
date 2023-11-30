using AutoMapper;
using LtbDb.Core.Interfaces;
using LtbDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;

namespace LtbDb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Policy = "AdministratorOnly")]
	public class HomeController : Controller
	{
		private readonly ILogger<HomeController> Log;

		private readonly IMapper Mapper;

		private readonly IBookService BookService;

		private readonly ICategoryService CategoryService;

		private readonly ITagService TagService;

		private readonly IMaintenanceService MaintenanceService;

		public HomeController(
			ILogger<HomeController> log,
			IMapper mapper,
			IBookService book,
			ICategoryService category,
			ITagService tag,
			IMaintenanceService maintenance)
		{
			Log = log;
			Mapper = mapper;
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

		[HttpPost]
		public async Task<IActionResult> Export(ExportModel model)
		{
			var filterCategory = model.Category ?? String.Empty;
			var filterTag = model.Tag ?? String.Empty;

			// add header to force it as download
			Response.Headers["Content-Disposition"] =
				$"attachment; filename=ltbdb-export-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}{(!String.IsNullOrEmpty(filterCategory) ? $"-category-{filterCategory}" : "")}{(!String.IsNullOrEmpty(filterTag) ? $"-tag-{filterTag}" : "")}.json";

			var _books = await BookService.GetByFilterAsync(filterCategory, filterTag);
			var books = Mapper.Map<BookModel[]>(_books);

			return Json(books, new JsonSerializerSettings
			{
				Formatting = Formatting.Indented
			});
		}

		[HttpGet]
		public async Task<IActionResult> Stats()
		{
			return Json(await MaintenanceService.GetStatisticsAsync(), new JsonSerializerSettings { Formatting = Formatting.Indented });
		}
	}
}
