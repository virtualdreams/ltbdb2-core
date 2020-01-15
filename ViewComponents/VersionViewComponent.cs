using Microsoft.AspNetCore.Mvc;
using ltbdb.Core.Helpers;

namespace ltbdb.ViewComponents
{
	public class VersionViewComponent : ViewComponent
	{
		public VersionViewComponent()
		{ }

		public IViewComponentResult Invoke() => Content($"{ApplicationVersion.InfoVersion()}");
	}
}
