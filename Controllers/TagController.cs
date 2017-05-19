using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.Controllers
{
	public class TagController : Controller
	{
		private readonly IMapper Mapper;
		private readonly IOptions<Settings> Settings;
		private readonly BookService Book;
		private readonly TagService Tag;

		public TagController(IMapper mapper, IOptions<Settings> settings, BookService book, TagService tag)
		{
			Mapper = mapper;
			Settings = settings;
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
			var _page = _books.Skip(ofs ?? 0).Take(Settings.Value.ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_books);
			var offset = new PageOffset(ofs ?? 0, Settings.Value.ItemsPerPage, _books.Count());

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
