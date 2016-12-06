using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using ltbdb.Core.Services;

namespace ltbdb.Api
{
    [Authorize(Policy = "AdministratorOnly")]
	[Route("api/[controller]/[action]")]
	public class StatsController : Controller
	{
		private readonly IMapper Mapper;
		private readonly BookService Book;
		private readonly CategoryService Category;
		private readonly TagService Tag;

		public StatsController(IMapper mapper, BookService book, CategoryService category, TagService tag)
		{
			Mapper = mapper;
			Book = book;
			Category = category;
			Tag = tag;
		}

		[HttpGet]
		public IActionResult List()
		{
			var _books = Book.Get().Count();
			var _categories = Category.Get().Count();
			var _stories = Book.Get().SelectMany(s => s.Stories).Distinct().Count();
			var _tags = Tag.Get().Count();

			var _stats = new
			{
				Books = _books,
				Categories = _categories,
				Stories = _stories,
				Tags = _tags
			};

			return Json(_stats, new JsonSerializerSettings{ Formatting = Formatting.Indented } );
		}
	}
}
