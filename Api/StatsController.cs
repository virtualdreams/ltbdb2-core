using ltbdb.Core.Services;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ltbdb.Api
{
    [Authorize(Policy = "AdministratorOnly")]
	[Route("api/[controller]/[action]")]
	public class StatsController : Controller
	{
		//private static readonly ILog Log = LogManager.GetLogger(typeof(StatsController));

		private readonly BookService Book;
		private readonly CategoryService Category;
		private readonly TagService Tag;

		public StatsController(BookService book, CategoryService category, TagService tag)
		{
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
