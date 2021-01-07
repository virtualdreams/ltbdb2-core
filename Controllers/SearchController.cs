using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System.Linq;
using System.Threading.Tasks;
using System;
using ltbdb.Core.Interfaces;
using ltbdb.Models;

namespace ltbdb.Controllers
{
	public class SearchController : Controller
	{
		private readonly IMapper Mapper;
		private readonly Settings Options;
		private readonly ISearchService SearchService;

		public SearchController(IMapper mapper, IOptionsSnapshot<Settings> settings, ISearchService search)
		{
			Mapper = mapper;
			Options = settings.Value;
			SearchService = search;
		}

		[HttpGet]
		public async Task<IActionResult> Search(string q, int? ofs)
		{
			var _books = await SearchService.SearchAsync(q ?? String.Empty);
			var _page = _books.Skip(ofs ?? 0).Take(Options.ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var offset = new PageOffset(ofs ?? 0, Options.ItemsPerPage, _books.Count());

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