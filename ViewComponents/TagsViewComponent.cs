using System.Linq;
using ltbdb.Core.Services;
using ltbdb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ltbdb.ViewComponents
{
	public class TagsViewComponent : ViewComponent
	{
		private readonly TagService Tag;

		public TagsViewComponent(TagService tag)
		{
			Tag = tag;
		}

		public IViewComponentResult Invoke()
		{
			var _categories = Tag.Get().Take(5);

			var view = new TagViewContainer { Tags = _categories };

			return View(view);
		}
	}
}