using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Linq;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.Controllers
{
	public class TagController : Controller
	{
		private readonly IMapper Mapper;
		private readonly Settings Options;
		private readonly BookService Book;
		private readonly TagService Tag;

		public TagController(IMapper mapper, Settings settings, BookService book, TagService tag)
		{
			Mapper = mapper;
			Options = settings;
			Book = book;
			Tag = tag;
		}

		[HttpGet]
		public IActionResult Index()
		{
			var _tags = Tag.Get().OrderBy(o => o);

			var view = new TagViewContainer
			{
				Tags = _tags
			};

			return View(view);
		}

		[HttpGet]
		public IActionResult View(string id, int? ofs)
		{
			var _books = Book.GetByTag(id ?? String.Empty);
			var _page = _books.Skip(ofs ?? 0).Take(Options.ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_books);
			var offset = new PageOffset(ofs ?? 0, Options.ItemsPerPage, _books.Count());

			var view = new BookViewTagContainer
			{
				Books = books,
				Tag = id,
				PageOffset = offset
			};

			return View(view);
		}
	}
}
