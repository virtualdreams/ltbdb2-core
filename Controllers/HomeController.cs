using AutoMapper;
using ltbdb.Core.Services;
using ltbdb.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ltbdb.Core.Helpers;

namespace ltbdb.Controllers
{
    public class HomeController : Controller
    {
		private readonly IMapper Mapper;
		private readonly BookService Book;
		private readonly CategoryService Category;
		private readonly TagService Tag;

		public HomeController(IMapper mapper, BookService book, CategoryService category, TagService tag)
		{
			Mapper = mapper;
			Book = book;
			Category = category;
			Tag = tag;
		}

		[HttpGet]
        public IActionResult Index()
        {
			var _books = Book.GetRecentlyAdded();

			var books = Mapper.Map<BookModel[]>(_books);

			var view = new BookViewContainer { Books = books };

			return View(view);
        }

		//[ValidateInput(false)]
		[HttpGet]
		public IActionResult Search(string q, int? ofs)
		{
			var _books = Book.Search(q ?? String.Empty);
			var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var offset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

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
