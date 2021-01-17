using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System.Threading.Tasks;
using ltbdb.Core.Interfaces;
using ltbdb.Options;

namespace ltbdb.WebAPI.V1.Controllers
{
	[ApiController]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	[Authorize(Policy = "AdministratorOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	public class CategoryController : ControllerBase
	{
		private readonly IMapper Mapper;
		private readonly AppSettings AppSettings;
		private readonly IBookService BookService;
		private readonly ICategoryService CategoryService;
		private readonly ITagService TagService;

		public CategoryController(IMapper mapper, IOptionsSnapshot<AppSettings> settings, IBookService book, ICategoryService category, ITagService tag)
		{
			Mapper = mapper;
			AppSettings = settings.Value;
			BookService = book;
			CategoryService = category;
			TagService = tag;
		}

		[HttpGet]
		public async Task<IActionResult> GetAll()
		{
			var _categories = await CategoryService.GetAsync();

			return Ok(_categories);
		}
	}
}