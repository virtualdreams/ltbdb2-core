using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.ViewComponents
{
	public class TagsViewComponent : ViewComponent
	{
		private readonly TagService Tag;

		public TagsViewComponent(TagService tag)
		{
			Tag = tag;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var _categories = await Tag.GetAsync();

			var view = new TagViewContainer
			{
				Tags = _categories.Take(5)
			};

			return View(view);
		}
	}
}