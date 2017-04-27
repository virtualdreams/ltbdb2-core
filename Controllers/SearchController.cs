using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq;
using System;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.Controllers
{
	public class SearchController : Controller
	{
		private readonly IMapper Mapper;
		private readonly IOptions<Settings> Settings;
		private readonly BookService Book;
		private readonly CategoryService Category;
		private readonly TagService Tag;

		public SearchController(IMapper mapper, IOptions<Settings> settings, BookService book, CategoryService category, TagService tag)
		{
			Mapper = mapper;
			Settings = settings;
			Book = book;
			Category = category;
			Tag = tag;
		}

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

		[HttpGet]
		public IActionResult SearchTitle(string term)
		{
			return Json(Book.Suggestions(term ?? String.Empty), new JsonSerializerSettings{ Formatting = Formatting.Indented } );
		}

		[HttpGet]
		public IActionResult SearchCategory(string term)
		{
			return Json(Category.Suggestions(term ?? String.Empty), new JsonSerializerSettings{ Formatting = Formatting.Indented } );
		}

		[HttpGet]
		public IActionResult SearchTag(string term)
		{
			return Json(Tag.Suggestions(term ?? String.Empty), new JsonSerializerSettings{ Formatting = Formatting.Indented } );
		}
	}
}