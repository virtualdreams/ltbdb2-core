using AutoMapper;
using LtbDb.Core.Interfaces;
using LtbDb.Models;
using LtbDb.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace LtbDb.Controllers
{
	public class CategoryController : Controller
	{
		private readonly ILogger<CategoryController> Log;

		private readonly IMapper Mapper;

		private readonly AppSettings AppSettings;

		private readonly IBookService BookService;

		private readonly ICategoryService CategoryService;

		public CategoryController(
			ILogger<CategoryController> log,
			IMapper mapper,
			IOptionsSnapshot<AppSettings> settings,
			IBookService book,
			ICategoryService category)
		{
			Log = log;
			Mapper = mapper;
			AppSettings = settings.Value;
			BookService = book;
			CategoryService = category;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var _categories = await CategoryService.GetAsync();

			var view = new CategoryViewContainer
			{
				Categories = _categories
			};

			return View(view);
		}

		[HttpGet]
		public async Task<IActionResult> View(string id, int? ofs)
		{
			var _books = await BookService.GetByCategoryAsync(id ?? String.Empty);
			if (_books.Count() == 0)
				return NotFound();

			var _page = _books.Skip(ofs ?? 0).Take(AppSettings.ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var offset = new PageOffset(ofs ?? 0, AppSettings.ItemsPerPage, _books.Count());

			var view = new BookViewCategoryContainer
			{
				Books = books,
				Category = id,
				PageOffset = offset
			};

			return View(view);
		}
	}
}
