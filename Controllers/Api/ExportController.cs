using ltbdb.Core.Services;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ltbdb.Controllers.Api
{
    [Route("api/[controller]/[action]")]
	//[LogError(Order = 0)]
    public class ExportController : Controller
	{
		//private static readonly ILog Log = LogManager.GetLogger(typeof(ExportController));

		private readonly BookService Book;

		public ExportController(BookService book)
		{
			Book = book;
		}

		[HttpGet]
		public IActionResult List()
		{
			return Json(Book.Export(), new JsonSerializerSettings{ Formatting = Formatting.Indented});
			//return Ok(Book.Export());
		}
	}
}
