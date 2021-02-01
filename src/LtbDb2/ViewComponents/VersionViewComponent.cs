using Microsoft.AspNetCore.Mvc;

namespace LtbDb.ViewComponents
{
	public class VersionViewComponent : ViewComponent
	{
		public VersionViewComponent()
		{ }

		public IViewComponentResult Invoke() => Content($"{ApplicationVersion.InfoVersion()}");
	}
}
