using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System;
using ltbdb.Core.Services;

namespace ltbdb.WebAPI.Controllers.V1
{
	[ApiController]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	[Authorize(Policy = "AdministratorOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class ImageController : ControllerBase
	{
		private readonly IMapper Mapper;
		private readonly Settings Options;
		private readonly BookService BookService;
		private readonly CategoryService CategoryService;
		private readonly TagService TagService;
		private readonly ImageService ImageService;

		// public class FileUploadAPI
		// {
		// 	public IFormFile files { get; set; }
		// }

		public ImageController(IMapper mapper, IOptionsSnapshot<Settings> settings, BookService book, CategoryService category, TagService tag, ImageService image)
		{
			Mapper = mapper;
			Options = settings.Value;
			BookService = book;
			CategoryService = category;
			TagService = tag;
			ImageService = image;
		}

		[HttpGet("{id}")]
		public IActionResult GetById(int id)
		{
			var _book = BookService.GetById(id);
			if (_book == null)
				return NotFound();

			return Ok(new
			{
				Thumbnail = ImageService.GetImageWebPath(_book.Filename, ImageType.Thumbnail, true),
				Image = ImageService.GetImageWebPath(_book.Filename, ImageType.Normal, true)
			});
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