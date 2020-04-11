using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using ltbdb.Core.Services;

namespace ltbdb.WebAPI.Controllers.V1
{
	[ApiController]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	[Authorize(Policy = "AdministratorOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class CategoryController : ControllerBase
	{
		private readonly IMapper Mapper;
		private readonly Settings Options;
		private readonly BookService BookService;
		private readonly CategoryService CategoryService;
		private readonly TagService TagService;

		public CategoryController(IMapper mapper, IOptionsSnapshot<Settings> settings, BookService book, CategoryService category, TagService tag)
		{
			Mapper = mapper;
			Options = settings.Value;
			BookService = book;
			CategoryService = category;
			TagService = tag;
		}

		[HttpGet]
		public IActionResult GetAll()
		{
			var _categories = CategoryService.Get();
			return Ok(_categories);
		}
	}
}