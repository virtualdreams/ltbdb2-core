﻿using ltbdb.Core.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;

namespace ltbdb.Api
{
    [Authorize(Policy = "AdministratorOnly")]
	[Route("api/[controller]/[action]")]
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
		}
	}
}
