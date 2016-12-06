using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using ltbdb.Core.Services;

namespace ltbdb.Api
{
    [Route("api/[controller]/[action]")]
    public class SearchController : Controller
    {
		private readonly IMapper Mapper;
		private readonly BookService Book;
		private readonly CategoryService Category;
		private readonly TagService Tag;

		public SearchController(IMapper mapper, BookService book, CategoryService category, TagService tag)
		{
			Mapper = mapper;
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
