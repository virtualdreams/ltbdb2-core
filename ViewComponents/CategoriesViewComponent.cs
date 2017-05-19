using ltbdb.Core.Services;
using ltbdb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ltbdb.ViewComponents
{
	public class CategoriesViewComponent : ViewComponent
	{
		private readonly CategoryService Category;

		public CategoriesViewComponent(CategoryService category)
		{
			Category = category;
		}

		public IViewComponentResult Invoke()
		{
			var _categories = Category.Get();

			var view = new CategoryViewContainer { Categories = _categories };

			return View(view);
		}
	}
}