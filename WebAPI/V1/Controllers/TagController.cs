using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Net.Mime;
using ltbdb.Core.Services;
using ltbdb.WebAPI.V1.Contracts.Responses;

namespace ltbdb.WebAPI.V1.Controllers
{
	[ApiController]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	[Authorize(Policy = "AdministratorOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class TagController : ControllerBase
	{
		private readonly IMapper Mapper;
		private readonly Settings Options;
		private readonly BookService BookService;
		private readonly CategoryService CategoryService;
		private readonly TagService TagService;

		public TagController(IMapper mapper, IOptionsSnapshot<Settings> settings, BookService book, CategoryService category, TagService tag)
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
			var _tags = TagService.Get();

			return Ok(_tags);
		}
	}
}