﻿using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.Controllers
{
	public class HomeController : Controller
	{
		private readonly IMapper Mapper;
		private readonly IOptions<Settings> Settings;
		private readonly BookService Book;

		public HomeController(IMapper mapper, IOptions<Settings> settings, BookService book)
		{
			Mapper = mapper;
			Settings = settings;
			Book = book;
		}

		[HttpGet]
		public IActionResult Index()
		{
			var _books = Book.GetRecentlyAdded(Settings.Value.RecentItems);

			var books = Mapper.Map<BookModel[]>(_books);

			var view = new BookViewContainer { Books = books };

			return View(view);
		}

		public IActionResult Error(int? code)
		{
			switch (code ?? 0)
			{
				case 404:
					return View("PageNotFound");

				default:
					return View();
			}
		}
	}
}
