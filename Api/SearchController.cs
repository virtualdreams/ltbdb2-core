using ltbdb.Core.Services;
using System;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ltbdb.Api
{
    [Route("api/[controller]/[action]")]
    public class SearchController : Controller
    {
		//private static readonly ILog Log = LogManager.GetLogger(typeof(SearchController));

		private readonly BookService Book;
		private readonly CategoryService Category;
		private readonly TagService Tag;

		public SearchController(BookService book, CategoryService category, TagService tag)
		{
			Book = book;
			Category = category;
			Tag = tag;
		}

		[HttpGet]
		public IActionResult Title(string term)
		{
			return Json(Book.Suggestions(term ?? String.Empty), new JsonSerializerSettings{ Formatting = Formatting.Indented } );
		}

		[HttpGet]
		public IActionResult Categories(string term)
		{
			return Json(Category.Suggestions(term ?? String.Empty), new JsonSerializerSettings{ Formatting = Formatting.Indented } );
		}

		[HttpGet]
		public IActionResult Tags(string term)
		{
			return Json(Tag.Suggestions(term ?? String.Empty), new JsonSerializerSettings{ Formatting = Formatting.Indented } );
		}
    }
}
