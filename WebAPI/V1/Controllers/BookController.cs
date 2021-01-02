using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using System;
using ltbdb.Core.Interfaces;
using ltbdb.Core.Models;
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
		private readonly IBookService BookService;
		private readonly ICategoryService CategoryService;
		private readonly ITagService TagService;

		public BookController(IMapper mapper, IOptionsSnapshot<Settings> settings, IBookService book, ICategoryService category, ITagService tag)
		{
			Mapper = mapper;
			Options = settings.Value;
			BookService = book;
			CategoryService = category;
			TagService = tag;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll(string category, string tag)
		{
			var filterCategory = category ?? String.Empty;
			var filterTag = tag ?? String.Empty;

			var _books = await BookService.GetByFilterAsync(filterCategory, filterTag);

			return Ok(Mapper.Map<List<BookResponse>>(_books));
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id)
		{
			var _book = await BookService.GetByIdAsync(id);
			if (_book == null)
				return NotFound();

			return Ok(Mapper.Map<BookResponse>(_book));
		}

		[HttpPost]
		public async Task<IActionResult> Post([FromBody] BookRequest model)
		{
			try
			{
				var _book = Mapper.Map<Book>(model);

				var book = await BookService.CreateAsync(_book);

				return Created(new Uri($"{Request.Scheme}://{Request.Host}{Request.Path}/{book.Id}", UriKind.Absolute), null);
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromBody] BookRequest model)
		{
			try
			{
				var _book = await BookService.GetByIdAsync(id);
				if (_book == null)
					return NotFound();

				var book = Mapper.Map<Book>(model);
				book.Id = id;
				await BookService.UpdateAsync(book);

				return NoContent();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpDelete("{id}")]
		public async Task<IActionResult> Delete(int id)
		{
			try
			{
				var _book = await BookService.GetByIdAsync(id);
				if (_book == null)
					return NotFound();

				await BookService.DeleteAsync(id);

				return NoContent();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}
	}
}