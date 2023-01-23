using AutoMapper;
using LtbDb.Core.Interfaces;
using LtbDb.Models;
using LtbDb.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace LtbDb.Controllers
{
	public class SearchController : Controller
	{
		private readonly ILogger<SearchController> Log;

		private readonly IMapper Mapper;

		private readonly AppSettings AppSettings;

		private readonly ISearchService SearchService;

		public SearchController(
			ILogger<SearchController> log,
			IMapper mapper,
			IOptionsSnapshot<AppSettings> settings,
			ISearchService search)
		{
			Log = log;
			Mapper = mapper;
			AppSettings = settings.Value;
			SearchService = search;
		}

		[HttpGet]
		public async Task<IActionResult> Search(string q, int? ofs)
		{
			var _books = await SearchService.SearchAsync(q ?? String.Empty);
			var _page = _books.Skip(ofs ?? 0).Take(AppSettings.ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var offset = new PageOffset(ofs ?? 0, AppSettings.ItemsPerPage, _books.Count());

			var view = new BookViewSearchContainer
			{
				Books = books,
				Query = q,
				PageOffset = offset
			};

			return View(view);
		}

		[HttpGet]
		public async Task<IActionResult> SearchTitle(string term)
		{
			return Json(await SearchService.SearchSuggestionsAsync(term ?? String.Empty), new JsonSerializerSettings { Formatting = Formatting.Indented });
		}

		[HttpGet]
		public async Task<IActionResult> SearchCategory(string term)
		{
			return Json(await SearchService.CategorySuggestionsAsync(term ?? String.Empty), new JsonSerializerSettings { Formatting = Formatting.Indented });
		}

		[HttpGet]
		public async Task<IActionResult> SearchTag(string term)
		{
			return Json(await SearchService.TagSuggestionsAsync(term ?? String.Empty), new JsonSerializerSettings { Formatting = Formatting.Indented });
		}
	}
}