using AutoMapper;
using LtbDb.Core.Interfaces;
using LtbDb.Core.Models;
using LtbDb.Options;
using LtbDb.WebAPI.V1.Contracts.Requests;
using LtbDb.WebAPI.V1.Contracts.Responses;
using LtbDb.WebAPI.V1.Filter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;
using System;

namespace LtbDb.WebAPI.V1.Controllers
{
	[ApiController]
	[ApiExplorerSettings(GroupName = "v1")]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	[Authorize(Policy = "AdministratorOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ValidationFilter]
	public class BookController : ControllerBase
	{
		private readonly ILogger<BookController> Log;

		private readonly IMapper Mapper;

		private readonly AppSettings AppSettings;

		private readonly IBookService BookService;

		private readonly ICategoryService CategoryService;

		private readonly ITagService TagService;

		public BookController(
			ILogger<BookController> log,
			IMapper mapper,
			IOptionsSnapshot<AppSettings> settings,
			IBookService book,
			ICategoryService category,
			ITagService tag)
		{
			Log = log;
			Mapper = mapper;
			AppSettings = settings.Value;
			BookService = book;
			CategoryService = category;
			TagService = tag;
		}

		/// <summary>
		/// Get all books.
		/// </summary>
		/// <param name="category">Filter by category.</param>
		/// <param name="tag">Filter by tag.</param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(IList<BookResponse>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll(string category, string tag)
		{
			var filterCategory = category ?? String.Empty;
			var filterTag = tag ?? String.Empty;

			var _books = await BookService.GetByFilterAsync(filterCategory, filterTag);

			return Ok(Mapper.Map<IList<BookResponse>>(_books));
		}

		/// <summary>
		/// Get a book by id.
		/// </summary>
		/// <param name="id">The book id.</param>
		/// <returns></returns>
		[HttpGet("{id}")]
		[ProducesResponseType(typeof(BookResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
		public async Task<IActionResult> GetById(int id)
		{
			var _book = await BookService.GetByIdAsync(id);
			if (_book == null)
				return NotFound();

			return Ok(Mapper.Map<BookResponse>(_book));
		}

		/// <summary>
		/// Add a new book.
		/// </summary>
		/// <param name="model">The book data.</param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(StatusCodes.Status201Created)]
		[ProducesResponseType(typeof(IList<ErrorResponse>), StatusCodes.Status400BadRequest)]
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

		/// <summary>
		/// Update a book.
		/// </summary>
		/// <param name="id">The book id.</param>
		/// <param name="model">The book data.</param>
		/// <returns></returns>
		[HttpPut("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(typeof(IList<ErrorResponse>), StatusCodes.Status400BadRequest)]
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

		/// <summary>
		/// Delete a book.
		/// </summary>
		/// <param name="id">The book id.</param>
		/// <returns></returns>
		[HttpDelete("{id}")]
		[ProducesResponseType(StatusCodes.Status204NoContent)]
		[ProducesResponseType(StatusCodes.Status404NotFound)]
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