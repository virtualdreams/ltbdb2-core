using AutoMapper;
using ltbdb.Core.Services;
using ltbdb.Models;
using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using ltbdb.Core.Helpers;

namespace ltbdb.Controllers
{
    public class TagController : Controller
    {
        //private static readonly ILog Log = LogManager.GetLogger(typeof(TagController));

        private readonly BookService Book;
        private readonly TagService Tag;

        public TagController(BookService book, TagService tag)
        {
            Book = book;
            Tag = tag;
        }

        [HttpGet]
        public IActionResult Index()
        {
            var _tags = Tag.Get().OrderBy(o => o);

            var view = new TagViewContainer
            {
                Tags = _tags
            };

            return View(view);
        }

        [HttpGet]
        public IActionResult View(string id, int? ofs)
        {
            var _books = Book.GetByTag(id ?? String.Empty);
            var _page = _books.Skip(ofs ?? 0).Take(GlobalConfig.Get().ItemsPerPage);

            var books = Mapper.Map<BookModel[]>(_books);
            var offset = new PageOffset(ofs ?? 0, GlobalConfig.Get().ItemsPerPage, _books.Count());

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
