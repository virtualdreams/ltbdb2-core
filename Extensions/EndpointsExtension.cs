using Microsoft.AspNetCore.Builder;

namespace ltbdb.Extensions
{
	public static class EndpointsExtension
	{
		public static IApplicationBuilder AddEndpoints(this IApplicationBuilder app)
		{
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllerRoute(
					name: "areaExport",
					pattern: "{area:exists}/export",
					defaults: new { area = "Admin", controller = "Home", action = "Export" }
				);

				endpoints.MapControllerRoute(
					name: "areaStats",
					pattern: "{area:exists}/stats",
					defaults: new { area = "Admin", controller = "Home", action = "Stats" }
				);

				endpoints.MapControllerRoute(
					name: "areaRoute",
					pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
				);

				endpoints.MapControllerRoute(
					name: "search",
					pattern: "search",
					defaults: new { controller = "Search", action = "Search" }
				);

				endpoints.MapControllerRoute(
					name: "searchTitle",
					pattern: "search/title",
					defaults: new { controller = "Search", action = "SearchTitle" }
				);

				endpoints.MapControllerRoute(
					name: "searchCategory",
					pattern: "search/category",
					defaults: new { controller = "Search", action = "SearchCategory" }
				);

				endpoints.MapControllerRoute(
					name: "searchTag",
					pattern: "search/tag",
					defaults: new { controller = "Search", action = "SearchTag" }
				);

				endpoints.MapControllerRoute(
					name: "view",
					pattern: "book/{id?}/{slug?}",
					defaults: new { controller = "Book", action = "View" },
					constraints: new { id = @"\d+" }
				);

				endpoints.MapControllerRoute(
					name: "create",
					pattern: "create",
					defaults: new { controller = "Book", action = "Create" }
				);

				endpoints.MapControllerRoute(
					name: "tags",
					pattern: "tags/{ofs?}",
					defaults: new { controller = "Tag", action = "Index" }
				);

				endpoints.MapControllerRoute(
					name: "tag",
					pattern: "tag/{id?}/{ofs?}",
					defaults: new { controller = "Tag", action = "View" }
				);

				endpoints.MapControllerRoute(
					name: "categories",
					pattern: "categories/{ofs?}",
					defaults: new { controller = "Category", action = "Index" }
				);

				endpoints.MapControllerRoute(
					name: "category",
					pattern: "category/{id?}/{ofs?}",
					defaults: new { controller = "Category", action = "View" }
				);

				endpoints.MapControllerRoute(
					name: "login",
					pattern: "login",
					defaults: new { controller = "Account", action = "Login" }
				);

				endpoints.MapControllerRoute(
					name: "logout",
					pattern: "logout",
					defaults: new { controller = "Account", action = "Logout" }
				);

				endpoints.MapControllerRoute(
					name: "error",
					pattern: "error/{code?}",
					defaults: new { controller = "home", action = "error" }
				);

				endpoints.MapControllerRoute(
					name: "default",
					pattern: "{controller=Home}/{action=Index}/{id?}"
				);
			});

			return app;
		}
	}
}