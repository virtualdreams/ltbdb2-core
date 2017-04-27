using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using ltbdb.Core.Helpers;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.Controllers
{
    public class BookController : Controller
    {
		private readonly IMapper Mapper;
		private readonly BookService Book;

		public BookController(IMapper mapper, BookService book)
		{
			Mapper = mapper;
			Book = book;
		}

		[HttpGet]
		public IActionResult View(ObjectId id)
		{
			var _book = Book.GetById(id);
			if (_book == null)
				return new StatusCodeResult(404);

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

			var book = Mapper.Map<BookWriteModel>(_book);
			book.Number = null;

			var view = new BookEditContainer
			{
				Book = book
			};
			
			return View("Edit", view);
		}

		[Authorize]
		[HttpGet]
		public IActionResult Edit(ObjectId id)
		{
			var _book = Book.GetById(id);
			if (_book == null)
				return new StatusCodeResult(404);

			var book = Mapper.Map<BookWriteModel>(_book);

			var view = new BookEditContainer
			{
				Book = book
			};

			return View("Edit", view);
		}

		[Authorize]
		[HttpPost]
		public IActionResult Edit(BookWriteModel model)
		{
			if(ModelState.IsValid)
			{
				try
				{
					var book = Mapper.Map<Book>(model);
					var id = ObjectId.Empty;
					if (book.Id == ObjectId.Empty)
					{
						id = Book.Create(book);
					}
					else
					{
						Book.Update(book);
						id = book.Id;
					}

					// save image
					if (model.Image != null || model.Remove)
					{
						if (model.Remove)
						{
							Book.SetImage(id, null);
						}
						else
						{
							Book.SetImage(id, model.Image.OpenReadStream());
						}
					}

					return RedirectToAction("view", "book", new { id = UrlHelper.ToSlug(id, 100, "Nr.", book.Number.ToString(), book.Title) });
				}
				catch(Exception ex)
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
		public IActionResult Delete(ObjectId id)
		{
			try
			{
				Book.Delete(id);

				return Json(new { Success = true, Error = "" });
			}
			catch(Exception ex)
			{
				return Json(new { Success = false, Error = ex.Message });
			}
		}
    }
}
