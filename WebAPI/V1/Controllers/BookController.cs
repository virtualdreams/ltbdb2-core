using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Mime;
using System;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using ltbdb.WebAPI.V1.Contracts.Requests;
using ltbdb.WebAPI.V1.Contracts.Responses;
using ltbdb.WebAPI.V1.Filter;

namespace ltbdb.WebAPI.V1.Controllers
{
	[ApiController]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	[Authorize(Policy = "AdministratorOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ValidationFilter]
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

			return Ok(Mapper.Map<List<BookResponse>>(_books));
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var _book = BookService.GetById(id);
			if (_book == null)
				return NotFound();

			return Ok(Mapper.Map<BookResponse>(_book));
		}

		[HttpPost]
		public IActionResult Post([FromBody]BookRequest model)
		{
			try
			{
				var _book = Mapper.Map<Book>(model);

				var book = BookService.Create(_book);

				return Created(new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}/{book.Id}", UriKind.Absolute), null);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpPut("{id}")]
		public IActionResult Put(int id, [FromBody]BookRequest model)
		{
			try
			{
				var _book = BookService.GetById(id);
				if (_book == null)
					return NotFound();

				var book = Mapper.Map<Book>(model);
				book.Id = id;
				BookService.Update(book);

				return NoContent();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
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

				return NoContent();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}
	}
}