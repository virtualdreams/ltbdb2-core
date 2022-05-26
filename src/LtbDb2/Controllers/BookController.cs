using AutoMapper;
using LtbDb.Core.Interfaces;
using LtbDb.Core.Models;
using LtbDb.Extensions;
using LtbDb.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using System;

namespace LtbDb.Controllers
{
	public class BookController : Controller
	{
		private readonly IMapper Mapper;
		private readonly IBookService BookService;

		public BookController(IMapper mapper, IBookService book)
		{
			Mapper = mapper;
			BookService = book;
		}

		[HttpGet]
		public async Task<IActionResult> View(int id)
		{
			var _book = await BookService.GetByIdAsync(id);
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
		public async Task<IActionResult> Edit(int id)
		{
			var _book = await BookService.GetByIdAsync(id);
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
		public async Task<IActionResult> Edit(BookPostModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var book = Mapper.Map<Book>(model);
					var _id = 0;
					if (book.Id == 0)
					{
						var _book = await BookService.CreateAsync(book);
						_id = _book.Id;
					}
					else
					{
						await BookService.UpdateAsync(book);
						_id = book.Id;
					}

					// save image
					if (model.Image != null || model.Remove)
					{
						if (model.Remove)
						{
							await BookService.SetImageAsync(_id, null);
						}
						else
						{
							await BookService.SetImageAsync(_id, model.Image.OpenReadStream());
						}
					}

					return RedirectToAction("view", "book", new { id = _id, slug = $"Nr. {book.Number} {book.Title}".ToSlug() });
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

			return View(view);
		}

		[Authorize]
		[SkipStatusCodePages]
		[HttpPost]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				await BookService.DeleteAsync(id);

				return Json(new { Success = true, Error = "" });
			}
			catch (Exception ex)
			{
				return Json(new { Success = false, Error = ex.Message });
			}
		}
	}
}
