using LtbDb.Core.Interfaces;
using LtbDb.Models;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace LtbDb.ViewComponents
{
	public class CategoriesViewComponent : ViewComponent
	{
		private readonly ICategoryService CategoryService;

		public CategoriesViewComponent(ICategoryService category)
		{
			CategoryService = category;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var _categories = await CategoryService.GetAsync();

			var view = new CategoryViewContainer
			{
				Categories = _categories
			};

			return View(view);
		}
	}
}