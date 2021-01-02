using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;
using ltbdb.Core.Interfaces;
using ltbdb.Models;

namespace ltbdb.Controllers
{
	public class HomeController : Controller
	{
		private readonly IMapper Mapper;
		private readonly Settings Options;
		private readonly IBookService BookService;

		public HomeController(IMapper mapper, IOptionsSnapshot<Settings> settings, IBookService book)
		{
			Mapper = mapper;
			Options = settings.Value;
			BookService = book;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var _books = await BookService.GetRecentlyAddedAsync(Options.RecentItems);

			var books = Mapper.Map<BookModel[]>(_books);

			var view = new BookViewContainer
			{
				Books = books
			};

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
