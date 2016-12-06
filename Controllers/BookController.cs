using AutoMapper;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using ltbdb.Models;
using MongoDB.Bson;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

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
			if (!ModelState.IsValid)
			{
				var view = new BookEditContainer
				{
					Book = model
				};

				return View("Edit", view);
			}

			var book = Mapper.Map<Book>(model);
			var id = ObjectId.Empty;
			if (book.Id == ObjectId.Empty)
				id = Book.Create(book);
			else
				id = Book.Update(book);

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

			return RedirectToAction("view", "book", new { id = id });
		}

		[Authorize]
		//[IsAjaxRequest]
		[HttpPost]
		public IActionResult Delete(ObjectId id)
		{
			var _result = Book.Delete(id);

			return Json(new { Success = _result }, new JsonSerializerSettings { Formatting = Formatting.Indented });
		}
    }
}
