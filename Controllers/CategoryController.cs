using AutoMapper;
using ltbdb.Core.Services;
using ltbdb.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ltbdb.Core.Helpers;

namespace ltbdb.Controllers
{
    //[LogError(Order = 0)]
	//[HandleError(View = "Error", Order=99)]
    public class CategoryController : Controller
    {
		//private static readonly ILog Log = LogManager.GetLogger(typeof(CategoryController));

		private readonly BookService Book;

		public CategoryController(BookService book)
		{
			Book = book;
		}

		[HttpGet]
        public IActionResult Index(int? ofs)
        {
			var _books = Book.Get().OrderBy(o => o.Category);
			var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var offset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

			var view = new BookViewAllContainer
			{
				Books = books,
				PageOffset = offset
			};

			return View(view);
        }

		[HttpGet]
		public IActionResult View(string id, int? ofs)
		{
			var _books = Book.GetByCategory(id ?? String.Empty);
			if (_books.Count() == 0)
				return new StatusCodeResult(404);

			var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var offset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

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
