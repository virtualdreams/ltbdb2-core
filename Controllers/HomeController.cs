using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.Controllers
{
    public class HomeController : Controller
    {
		private readonly IMapper Mapper;
		private readonly IOptions<Settings> Settings;
		private readonly BookService Book;
		private readonly CategoryService Category;
		private readonly TagService Tag;

		public HomeController(IMapper mapper, IOptions<Settings> settings, BookService book, CategoryService category, TagService tag)
		{
			Mapper = mapper;
			Settings = settings;
			Book = book;
			Category = category;
			Tag = tag;
		}

		[HttpGet]
        public IActionResult Index()
        {
			var _books = Book.GetRecentlyAdded(Settings.Value.RecentItems);

			var books = Mapper.Map<BookModel[]>(_books);

			var view = new BookViewContainer { Books = books };

			return View(view);
        }

		//[ValidateInput(false)]
		[HttpGet]
		public IActionResult Search(string q, int? ofs)
		{
			var _books = Book.Search(q ?? String.Empty);
			var _page = _books.Skip(ofs ?? 0).Take(Settings.Value.ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var offset = new PageOffset(ofs ?? 0, Settings.Value.ItemsPerPage, _books.Count());

			var view = new BookViewSearchContainer
			{
				Books = books,
				Query = q,
				PageOffset = offset
			};

			return View(view);
		}
    }
}
