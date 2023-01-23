using AutoMapper;
using LtbDb.Core.Interfaces;
using LtbDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace LtbDb.Areas.Admin.Controllers
{
	[Area("Admin")]
	[Authorize(Policy = "AdministratorOnly")]
	public class CategoryController : Controller
	{
		private readonly ILogger<CategoryController> Log;

		private readonly IMapper Mapper;

		private readonly IBookService BookService;

		private readonly ICategoryService CategoryService;

		public CategoryController(
			ILogger<CategoryController> log,
			IMapper mapper,
			IBookService book,
			ICategoryService category)
		{
			Log = log;
			Mapper = mapper;
			BookService = book;
			CategoryService = category;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var _categories = await CategoryService.GetAsync();

			var view = new CategoryViewContainer
			{
				Categories = _categories.OrderBy(s => s)
			};

			return View(view);
		}

		[HttpPost]
		public async Task<IActionResult> Move(string from, string to)
		{
			await CategoryService.RenameAsync(from ?? String.Empty, to ?? String.Empty);

			return RedirectToAction("index", "category", new { area = "admin" });
		}
	}
}
