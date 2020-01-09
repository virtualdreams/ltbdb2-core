using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.WebAPI.Controllers
{
	[Produces("application/json")]
	[Route("api/v1/[controller]")]
	[Authorize(Policy = "AdministratorOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class BookController : Controller
	{
		private readonly IMapper Mapper;
		private readonly Settings Options;
		private readonly BookService BookService;
		private readonly CategoryService CategoryService;
		private readonly TagService TagService;

		public BookController(IMapper mapper, IOptionsSnapshot<Settings> settings, BookService book, CategoryService category, TagService tag)
		{
			Mapper = mapper;
			Options = settings.Value;
			BookService = book;
			CategoryService = category;
			TagService = tag;
		}

		[HttpGet]
		public IActionResult GetAll(string filter, string category)
		{
			var _books = BookService.Get();
			return Ok(Mapper.Map<BookModel[]>(_books));
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var _book = BookService.GetById(id);
			if (_book == null)
				return NotFound();

			return Ok(Mapper.Map<BookModel>(_book));
		}

		[HttpPost("{id}")]
		public IActionResult Post(int id, [FromBody]BookPostApiModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var _book = BookService.GetById(id);
					if (_book == null)
						return NotFound();

					var book = Mapper.Map<Book>(model);
					book.Id = id;
					BookService.Update(book);

					return Ok();
				}
				catch (Exception)
				{
					return StatusCode(500);
				}
			}

			return BadRequest();
		}

		[HttpPut]
		public IActionResult Put([FromBody]BookPostApiModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					var _book = Mapper.Map<Book>(model);

					var book = BookService.Create(_book);

					return Ok(new { Id = book.Id });
				}
				catch (Exception)
				{
					return StatusCode(500);
				}
			}

			return BadRequest();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(int id)
		{
			try
			{
				var _book = BookService.GetById(id);
				if (_book == null)
					return NotFound();

				BookService.Delete(id);

				return Ok();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}
	}
}