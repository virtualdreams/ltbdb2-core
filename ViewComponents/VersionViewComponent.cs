using Microsoft.AspNetCore.Mvc;
using ltbdb.Extensions;

namespace ltbdb.ViewComponents
{
	public class VersionViewComponent : ViewComponent
	{
		public VersionViewComponent()
		{ }

		public IViewComponentResult Invoke() => Content($"{ApplicationVersion.InfoVersion()}");
	}
}
