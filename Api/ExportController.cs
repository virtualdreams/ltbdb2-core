using AutoMapper;
using ltbdb.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ltbdb.Api
{
    [Route("api/[controller]/[action]")]
    public class ExportController : Controller
	{
		private readonly IMapper Mapper;
		private readonly BookService Book;

		public ExportController(IMapper mapper, BookService book)
		{
			Mapper = mapper;
			Book = book;
		}

		[HttpGet]
		public IActionResult List()
		{
			return Json(Book.Export(), new JsonSerializerSettings{ Formatting = Formatting.Indented});
		}

		public IActionResult Download()
		{
			Response.Headers.Add("Content-Disposition", "attachment; filename=books.json");

			return Json(Book.Export(), new JsonSerializerSettings{ Formatting = Formatting.Indented});
		}
	}
}
