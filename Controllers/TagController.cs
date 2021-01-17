using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Linq;
using System.Threading.Tasks;
using System;
using ltbdb.Core.Interfaces;
using ltbdb.Models;
using ltbdb.Options;

namespace ltbdb.Controllers
{
	public class TagController : Controller
	{
		private readonly IMapper Mapper;
		private readonly AppSettings AppSettings;
		private readonly IBookService BookService;
		private readonly ITagService TagService;

		public TagController(IMapper mapper, IOptionsSnapshot<AppSettings> settings, IBookService book, ITagService tag)
		{
			Mapper = mapper;
			AppSettings = settings.Value;
			BookService = book;
			TagService = tag;
		}

		[HttpGet]
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
