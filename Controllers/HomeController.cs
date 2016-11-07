using System.Collections.Generic;
using AutoMapper;
using ltbdb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ltbdb.Controllers
{
	public class HomeController : Controller
	{
		private readonly DemoService Service;

		public HomeController(DemoService service)
		{
			Service = service;
		}

		public IActionResult Index()
		{
			Mapper.Initialize(cfg => cfg.CreateMap<Book, BookModel>());

			var _books = Service.Get();

			var books = Mapper.Map<IEnumerable<Book>, IEnumerable<BookModel>>(_books);
			
			return View(books);
		}
	}
}
