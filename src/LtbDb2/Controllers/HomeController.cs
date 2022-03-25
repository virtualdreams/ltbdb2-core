using AutoMapper;
using LtbDb.Core.Interfaces;
using LtbDb.Models;
using LtbDb.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;

namespace LtbDb.Controllers
{
	public class HomeController : Controller
	{
		private readonly IMapper Mapper;
		private readonly AppSettings AppSettings;
		private readonly IBookService BookService;

		public HomeController(IMapper mapper, IOptionsSnapshot<AppSettings> settings, IBookService book)
		{
			Mapper = mapper;
			AppSettings = settings.Value;
			BookService = book;
		}

		[HttpGet]
		public async Task<IActionResult> Index()
		{
			var _books = await BookService.GetRecentlyAddedAsync(AppSettings.RecentItems);

			var books = Mapper.Map<BookModel[]>(_books);

			var view = new BookViewContainer
			{
				Books = books
			};

			return View(view);
		}

		[HttpGet]
		public async Task<IActionResult> All(int? ofs)
		{
			var _books = await BookService.GetAsync();
			var _page = _books.Skip(ofs ?? 0).Take(AppSettings.ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var offset = new PageOffset(ofs ?? 0, AppSettings.ItemsPerPage, _books.Count());

			var view = new BookViewAllContainer
			{
				Books = books,
				PageOffset = offset
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
