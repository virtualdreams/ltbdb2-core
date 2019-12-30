using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using ltbdb.Core.Models;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.WebAPI.Controllers
{
	[Produces("application/json")]
	[Route("api/v1/[controller]")]
	[Authorize(Policy = "AdministratorOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class ImageController : Controller
	{
		private readonly IMapper Mapper;
		private readonly Settings Options;
		private readonly BookService BookService;
		private readonly CategoryService CategoryService;
		private readonly TagService TagService;

		// public class FileUploadAPI
		// {
		// 	public IFormFile files { get; set; }
		// }

		public ImageController(IMapper mapper, Settings settings, BookService book, CategoryService category, TagService tag)
		{
			Mapper = mapper;
			Options = settings;
			BookService = book;
			CategoryService = category;
			TagService = tag;
		}

		[HttpPost("{id}")]
		public IActionResult Post(int id, IFormFile image)
		{
			if (image != null && image.Length > 0)
			{
				try
				{
					var _book = BookService.GetById(id);
					if (_book == null)
						return NotFound();

					BookService.SetImage(_book.Id, image.OpenReadStream());

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
			var _book = BookService.GetById(id);
			if (_book == null)
				return NotFound();

			BookService.SetImage(_book.Id, null);

			return Ok();
		}
	}
}