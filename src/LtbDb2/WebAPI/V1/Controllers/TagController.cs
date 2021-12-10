using AutoMapper;
using LtbDb.Core.Interfaces;
using LtbDb.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
	public class TagController : ControllerBase
	{
		private readonly IMapper Mapper;
		private readonly AppSettings AppSettings;
		private readonly IBookService BookService;
		private readonly ICategoryService CategoryService;
		private readonly ITagService TagService;

		public TagController(IMapper mapper, IOptionsSnapshot<AppSettings> settings, IBookService book, ICategoryService category, ITagService tag)
		{
			Mapper = mapper;
			AppSettings = settings.Value;
			BookService = book;
			CategoryService = category;
			TagService = tag;
		}

		/// <summary>
		/// Get all tags.
		/// </summary>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(List<string>), StatusCodes.Status200OK)]
		public async Task<IActionResult> GetAll()
		{
			var _tags = await TagService.GetAsync();

			return Ok(_tags);
		}
	}
}