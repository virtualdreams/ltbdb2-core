using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using ltbdb.Extensions;
using ltbdb.Models;

namespace ltbdb.Controllers
{
	public class BookController : Controller
	{
		private readonly IMapper Mapper;
		private readonly BookService BookService;

		public BookController(IMapper mapper, BookService book)
		{
			Mapper = mapper;
			BookService = book;
		}

		[HttpGet]
		public IActionResult View(int id)
		{
			var _book = BookService.GetById(id);
			if (_book == null)
				return NotFound();

			var book = Mapper.Map<BookModel>(_book);

			var view = new BookViewDetailContainer
			{
				Book = book,
			};

			return View(view);
		}

		[Authorize]
		[HttpGet]
		public IActionResult Create()
		{
			var _book = new Book();

			var book = Mapper.Map<BookPostModel>(_book);
			book.Number = null;

			var view = new BookEditContainer
			{
				Book = book
			};

			return View("Edit", view);
		}

		[Authorize]
		[HttpGet]
		public IActionResult Edit(int id)
		{
			var _book = BookService.GetById(id);
			if (_book == null)
				return NotFound();

			var book = Mapper.Map<BookPostModel>(_book);

			var view = new BookEditContainer
			{
				Book = book
			};

			return View("Edit", view);
		}

		[Authorize]
		[HttpPost]
		public IActionResult Edit(BookPostModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var book = Mapper.Map<Book>(model);
					var _id = 0;
					if (book.Id == 0)
					{
						var _book = BookService.Create(book);
						_id = book.Id;
					}
					else
					{
						BookService.Update(book);
						_id = book.Id;
					}

					// save image
					if (model.Image != null || model.Remove)
					{
						if (model.Remove)
						{
							BookService.SetImage(_id, null);
						}
						else
						{
							BookService.SetImage(_id, model.Image.OpenReadStream());
						}
					}

					return RedirectToAction("view", "book", new { id = _id, slug = UrlHelper.ToSlug(100, "Nr.", book.Number.ToString(), book.Title) });
				}
				catch (Exception ex)
				{
					ModelState.AddModelError("error", ex.Message);
				}
			}

			var view = new BookEditContainer
			{
				Book = model
			};

			return View("Edit", view);
		}

		[Authorize]
		//[IsAjaxRequest]
		[HttpPost]
		public IActionResult Delete(int id)
		{
			try
			{
				BookService.Delete(id);

				return Json(new { Success = true, Error = "" });
			}
			catch (Exception ex)
			{
				return Json(new { Success = false, Error = ex.Message });
			}
		}
	}
}
