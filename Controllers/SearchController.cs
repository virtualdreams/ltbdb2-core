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
		private readonly IBookService BookService;
		private readonly ICategoryService CategoryService;
		private readonly ITagService TagService;

		public SearchController(IMapper mapper, IOptionsSnapshot<Settings> settings, IBookService book, ICategoryService category, ITagService tag)
		{
			Mapper = mapper;
			Options = settings.Value;
			BookService = book;
			CategoryService = category;
			TagService = tag;
		}

		[HttpGet]
		public async Task<IActionResult> Search(string q, int? ofs)
		{
			var _books = await BookService.SearchAsync(q ?? String.Empty);
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
		[SkipStatusCodePages]
		public async Task<IActionResult> SearchTitle(string term)
		{
			return Json(await BookService.SuggestionsAsync(term ?? String.Empty), new JsonSerializerSettings { Formatting = Formatting.Indented });
		}

		[HttpGet]
		[SkipStatusCodePages]
		public async Task<IActionResult> SearchCategory(string term)
		{
			return Json(await CategoryService.SuggestionsAsync(term ?? String.Empty), new JsonSerializerSettings { Formatting = Formatting.Indented });
		}

		[HttpGet]
		[SkipStatusCodePages]
		public async Task<IActionResult> SearchTag(string term)
		{
			return Json(await TagService.SuggestionsAsync(term ?? String.Empty), new JsonSerializerSettings { Formatting = Formatting.Indented });
		}
	}
}