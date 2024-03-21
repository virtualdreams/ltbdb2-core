using AutoMapper;
using LtbDb.Core.Interfaces;
using LtbDb.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace LtbDb.WebAPI.V1.Controllers
{
	[ApiController]
	[ApiExplorerSettings(GroupName = "v1")]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	[Authorize(Policy = "AdministratorOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	public class CategoryController : ControllerBase
	{
		private readonly ILogger<CategoryController> Log;

		private readonly IMapper Mapper;

		private readonly AppSettings AppSettings;

		private readonly IBookService BookService;

		private readonly ICategoryService CategoryService;

		private readonly ITagService TagService;

		public CategoryController(
			ILogger<CategoryController> log,
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
		/// Get all categories.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(IList<string>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll()
		{
			var _categories = await CategoryService.GetAsync();

			return Ok(_categories);
		}
	}
}