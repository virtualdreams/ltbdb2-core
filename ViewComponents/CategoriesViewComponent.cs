using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using ltbdb.Core.Services;
using ltbdb.Models;

namespace ltbdb.ViewComponents
{
	public class CategoriesViewComponent : ViewComponent
	{
		private readonly CategoryService Category;

		public CategoriesViewComponent(CategoryService category)
		{
			Category = category;
		}

		public async Task<IViewComponentResult> InvokeAsync()
		{
			var _categories = await Category.GetAsync();

			var view = new CategoryViewContainer
			{
				Categories = _categories
			};

			return View(view);
		}
	}
}