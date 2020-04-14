using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using ltbdb.Models;
using ltbdb.WebAPI.Contracts.V1.Requests;

namespace ltbdb.WebAPI.Controllers.V1
{
	[ApiController]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	[Authorize(Policy = "AdministratorOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class BookController : ControllerBase
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
		public IActionResult GetAll(string category, string tag)
		{
			var filterCategory = category ?? String.Empty;
			var filterTag = tag ?? String.Empty;

			var _books = BookService.GetByFilter(filterCategory, filterTag);
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

		[HttpPost]
		public IActionResult Post([FromBody]BookApiRequest model)
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

		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody]BookApiRequest model)
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