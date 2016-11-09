using ltbdb.Models;
using Microsoft.AspNetCore.Mvc;

namespace ltbdb.ViewComponents
{
    public class BookViewComponent : ViewComponent
    {
		public IViewComponentResult Invoke(BookModel model)
		{
			return View(model);
		}
	}
}