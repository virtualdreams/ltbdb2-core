using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;
using ltbdb.Core.Interfaces;
using ltbdb.Models;

namespace ltbdb.ViewComponents
{
	public class TagsViewComponent : ViewComponent
	{
		private readonly ITagService TagService;

		public TagsViewComponent(ITagService tag)
		{
			TagService = tag;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var _tags = await TagService.GetAsync();

			var view = new TagViewContainer
			{
				Tags = _tags.Take(5)
			};

			return View(view);
		}
	}
}