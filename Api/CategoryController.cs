using ltbdb.Core.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ltbdb.Api
{
    [Authorize]
	[Route("api/[controller]/[action]")]
	//[LogError(Order = 0)]
	public class CategoryController : Controller
	{
		//private static readonly ILog Log = LogManager.GetLogger(typeof(CategoryController));

		private readonly CategoryService Category;

		public CategoryController(CategoryService category)
		{
			Category = category;
		}

		[HttpGet]
		public IActionResult List()
		{
			return Json(Category.Get(), new JsonSerializerSettings{ Formatting = Formatting.Indented } );
			//return Category.Get();
		}
	}
}
