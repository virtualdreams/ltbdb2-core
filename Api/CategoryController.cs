using AutoMapper;
using ltbdb.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ltbdb.Api
{
    [Authorize(Policy = "AdministratorOnly")]
	[Route("api/[controller]/[action]")]
	public class CategoryController : Controller
	{
		private readonly IMapper Mapper;
		private readonly CategoryService Category;

		public CategoryController(IMapper mapper, CategoryService category)
		{
			Category = category;
		}

		[HttpGet]
		public IActionResult List()
		{
			return Json(Category.Get(), new JsonSerializerSettings{ Formatting = Formatting.Indented } );
		}
	}
}
