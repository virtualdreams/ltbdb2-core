using AutoMapper;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using ltbdb.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;
using System;
using System.Collections.Generic;

namespace ltbdb.WebAPI.Controllers
{
	[Produces("application/json")]
	[Route("api/v1/[controller]")]
	[Authorize(Policy = "AdministratorOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class BookController : Controller
	{
		private readonly IMapper Mapper;
		private readonly Settings Options;
		private readonly BookService Book;
		private readonly CategoryService Category;
		private readonly TagService Tag;

		public BookController(IMapper mapper, Settings settings, BookService book, CategoryService category, TagService tag)
		{
			Mapper = mapper;
			Options = settings;
			Book = book;
			Category = category;
			Tag = tag;
		}

		[HttpGet]
		public IActionResult GetAll(string filter, string category)
		{
			var _books = Book.Get();
			return Ok(Mapper.Map<BookModel[]>(_books));
		}

		[HttpGet("{id}")]
		public IActionResult GetById(ObjectId id)
		{
			var _book = Book.GetById(id);
			if(_book == null)
				return NotFound();

			return Ok(Mapper.Map<BookModel>(_book));
		}

		[HttpPost("{id}")]
		public IActionResult Post(ObjectId id, [FromBody]BookWriteApiModel model)
		{
			if(ModelState.IsValid)
			{
				try
				{
					var _book = Book.GetById(id);
					if(_book == null)
						return NotFound();

					var book = Mapper.Map<Book>(model);
					book.Id = id;
					Book.Update(book);

					return Ok();
				}
				catch(Exception)
				{
					return StatusCode(500);
				}
			}

			return BadRequest();
		}

		[HttpPut]
		public IActionResult Put([FromBody]BookWriteApiModel model)
		{
			if(ModelState.IsValid)
			{
				try
				{
					var book = Mapper.Map<Book>(model);

					var id = Book.Create(book);

					return Ok(new { Id = id.ToString() });
				}
				catch(Exception)
				{
					return StatusCode(500);
				}
			}

			return BadRequest();
		}

		[HttpDelete("{id}")]
		public IActionResult Delete(ObjectId id)
		{
			try
			{
				var _book = Book.GetById(id);
				if(_book == null)
					return NotFound();

				Book.Delete(id);

				return Ok();
			}
			catch(Exception)
			{
				return StatusCode(500);
			}
		}
	}
}