using AutoMapper;
using LtbDb.Core.Interfaces;
using LtbDb.Options;
using LtbDb.WebAPI.V1.Contracts.Requests;
using LtbDb.WebAPI.V1.Filter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Threading.Tasks;
using System;

namespace LtbDb.WebAPI.V1.Controllers
{
	[ApiController]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	[Authorize(Policy = "AdministratorOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ValidationFilter]
	public class ImageController : ControllerBase
	{
		private readonly IMapper Mapper;
		private readonly AppSettings AppSettings;
		private readonly IBookService BookService;
		private readonly ICategoryService CategoryService;
		private readonly ITagService TagService;
		private readonly IImageService ImageService;

		public ImageController(IMapper mapper, IOptionsSnapshot<AppSettings> settings, IBookService book, ICategoryService category, ITagService tag, IImageService image)
		{
			Mapper = mapper;
			AppSettings = settings.Value;
			BookService = book;
			CategoryService = category;
			TagService = tag;
			ImageService = image;
		}

		[HttpGet("{id}")]
		public async Task<IActionResult> GetById(int id, string type)
		{
			var _book = await BookService.GetByIdAsync(id);
			if (_book == null)
				return NotFound();

			var _thumbnail = String.Equals(type, "thumbnail");
			var _file = ImageService.GetPhysicalPath(_book.Filename, _thumbnail ? ImageType.Thumbnail : ImageType.Normal);
			if (String.IsNullOrEmpty(_file))
				return NotFound();

			return PhysicalFile(_file, MediaTypeNames.Image.Jpeg);
		}

		[HttpPut("{id}")]
		public async Task<IActionResult> Put(int id, [FromForm] ImageRequest model)
		{
			try
			{
				var _book = await BookService.GetByIdAsync(id);
				if (_book == null)
					return NotFound();

				await BookService.SetImageAsync(_book.Id, model.Image.OpenReadStream());

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
			var _book = await BookService.GetByIdAsync(id);
			if (_book == null)
				return NotFound();

			await BookService.SetImageAsync(_book.Id, null);

			return NoContent();
		}
	}
}