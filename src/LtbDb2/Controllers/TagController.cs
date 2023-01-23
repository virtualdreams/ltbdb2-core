using AutoMapper;
using LtbDb.Core.Interfaces;
using LtbDb.Features;
using LtbDb.Models;
using LtbDb.Options;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.FeatureManagement.Mvc;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace LtbDb.Controllers
{
	public class TagController : Controller
	{
		private readonly ILogger<TagController> Log;

		private readonly IMapper Mapper;

		private readonly AppSettings AppSettings;

		private readonly IBookService BookService;

		private readonly ITagService TagService;

		public TagController(
			ILogger<TagController> log,
			IMapper mapper,
			IOptionsSnapshot<AppSettings> settings,
			IBookService book,
			ITagService tag)
		{
			Log = log;
			Mapper = mapper;
			AppSettings = settings.Value;
			BookService = book;
			TagService = tag;
		}

		[HttpGet]
		[FeatureGate(FeatureFlags.ShowTagsPage)]
		public async Task<IActionResult> Index()
		{
			var _tags = await TagService.GetAsync();

			var view = new TagViewContainer
			{
				Tags = _tags
			};

			return View(view);
		}

		[HttpGet]
		public async Task<IActionResult> View(string id, int? ofs)
		{
			var _books = await BookService.GetByTagAsync(id ?? String.Empty);
			var _page = _books.Skip(ofs ?? 0).Take(AppSettings.ItemsPerPage);

			var books = Mapper.Map<BookModel[]>(_page);
			var offset = new PageOffset(ofs ?? 0, AppSettings.ItemsPerPage, _books.Count());

			var view = new BookViewTagContainer
			{
				Books = books,
				Tag = id,
				PageOffset = offset
			};

			return View(view);
		}
	}
}
