using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ltbdb.Core.Services;

namespace ltbdb.Api
{
    [Authorize(Policy = "AdministratorOnly")]
	[Route("api/[controller]/[action]")]
	public class TagController : Controller
	{
		private readonly IMapper Mapper;
		private readonly TagService Tag;

		public TagController(IMapper mapper, TagService tag)
		{
			Mapper = mapper;
			Tag = tag;
		}

		[HttpGet]
		public IActionResult List()
		{
			return Json(Tag.Get(), new JsonSerializerSettings{ Formatting = Formatting.Indented } );
		}
	}
}
