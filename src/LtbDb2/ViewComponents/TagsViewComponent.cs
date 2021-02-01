using LtbDb.Core.Interfaces;
using LtbDb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Threading.Tasks;

namespace LtbDb.ViewComponents
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