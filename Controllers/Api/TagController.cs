using ltbdb.Core.Services;
using System;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace ltbdb.Controllers.Api
{
    [Authorize]
	[Route("api/[controller]/[action]")]
	//[LogError(Order = 0)]
	public class TagController : Controller
	{
		//private static readonly ILog Log = LogManager.GetLogger(typeof(TagController));

		private readonly TagService Tag;

		public TagController(TagService tag)
		{
			Tag = tag;
		}

		[HttpGet]
		public IActionResult List()
		{
			return Json(Tag.Get(), new JsonSerializerSettings{ Formatting = Formatting.Indented } );
			//return Tag.Get();
		}
	}
}
