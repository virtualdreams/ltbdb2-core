using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.Controllers
{
	public class HomeController : Controller
	{
		private readonly IMapper Mapper;
		private readonly Settings Options;
		private readonly BookService BookService;

		public HomeController(IMapper mapper, Settings settings, BookService book)
		{
			Mapper = mapper;
			Options = settings;
			BookService = book;
		}

		[HttpGet]
		public IActionResult Index()
		{
			var _books = BookService.GetRecentlyAdded(Options.RecentItems);

			var books = Mapper.Map<BookModel[]>(_books);

			var view = new BookViewContainer { Books = books };

			return View(view);
		}

		public IActionResult Error(int? code)
		{
			switch (code ?? 0)
			{
				case 404:
					return View("PageNotFound");

				default:
					return View();
			}
		}
	}
}
