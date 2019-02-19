using Microsoft.AspNetCore.Mvc;
using ltbdb.Core.Helpers;
using ltbdb.Core.Services;

namespace ltbdb.ViewComponents
{
	public class VersionViewComponent : ViewComponent
	{
		public VersionViewComponent()
		{ }

		public IViewComponentResult Invoke() => Content($"{ApplicationVersion.InfoVersion()}");
	}
}
