using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Threading.Tasks;
using System;
using ltbdb.Core.Interfaces;
using ltbdb.Models;

namespace ltbdb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Policy = "AdministratorOnly")]
	public class HomeController : Controller
	{
		private readonly IMapper Mapper;
		private readonly IBookService BookService;
		private readonly ICategoryService CategoryService;
		private readonly ITagService TagService;
		private readonly IMaintenanceService MaintenanceService;

		public HomeController(IMapper mapper, IBookService book, ICategoryService category, ITagService tag, IMaintenanceService maintenance)
		{
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

		[HttpGet]
		public async Task<IActionResult> Export()
		{
			// add header to force it as download
			Response.Headers.Add("Content-Disposition", $"attachment; filename=ltbdb-export-{DateTime.Now.ToString("yyyyMMdd-HHmmss")}.json");

			var _books = await BookService.GetByFilterAsync(String.Empty, String.Empty);
			var books = Mapper.Map<BookModel[]>(_books);

			return Json(books, new JsonSerializerSettings { Formatting = Formatting.Indented });
		}

		[HttpGet]
		public async Task<IActionResult> Stats()
		{
			return Json(await MaintenanceService.GetStatisticsAsync(), new JsonSerializerSettings { Formatting = Formatting.Indented });
		}
	}
}
