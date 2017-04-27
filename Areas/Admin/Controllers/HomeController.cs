using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Linq;
using System;
using ltbdb.Core.Services;

namespace ltbdb.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Policy = "AdministratorOnly")]
    public class HomeController : Controller
    {
        private readonly BookService Book;
		private readonly CategoryService Category;
		private readonly TagService Tag;

        public HomeController(BookService book, CategoryService category, TagService tag)
        {
            Book = book;
			Category = category;
			Tag = tag;
        }

		[HttpGet]
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult List()
		{
			return Json(Book.Export(), new JsonSerializerSettings{ Formatting = Formatting.Indented});
		}

        [HttpGet]
        public IActionResult Export()
        {
            // add header to force it as download
            Response.Headers.Add("Content-Disposition", $"attachment; filename=ltbdb-export-{DateTime.Now.ToString("yyyyMMddHHmmss")}.json");

			return Json(Book.Export(), new JsonSerializerSettings{ Formatting = Formatting.Indented});
        }

        [HttpGet]
		public IActionResult Stats()
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
