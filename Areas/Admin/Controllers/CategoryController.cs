using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.Areas.Admin.Controllers
{
	[Area("Admin")]
    [Authorize(Policy = "AdministratorOnly")]
    public class CategoryController : Controller
    {
		private readonly IMapper Mapper;
		private readonly BookService Book;
		private readonly CategoryService Category;

		public CategoryController(IMapper mapper, BookService book, CategoryService category)
		{
			Mapper = mapper;
			Book = book;
			Category = category;
		}

		[HttpGet]
        public IActionResult Index()
        {
			var _categories = Category.Get().OrderBy(s => s);

			var view = new CategoryViewContainer
			{
				Categories = _categories
			};
			
			return View(view);
        }

		[HttpPost]
		public IActionResult Move(string from, string to)
		{
			Category.Rename(from ?? String.Empty, to ?? String.Empty);

			return Redirect("index");
		}
    }
}
