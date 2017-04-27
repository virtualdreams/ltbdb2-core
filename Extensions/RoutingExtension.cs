using Microsoft.AspNetCore.Builder;

namespace ltbdb.Extensions
{
	static public class RoutingExtensions
	{
		static public IApplicationBuilder AddRoutes(this IApplicationBuilder app)
		{
			app.UseMvc(routes => {
				routes.MapRoute(
					name: "areaExport",
    				template: "{area:exists}/export",
					defaults: new { area = "Admin", controller = "Home", action = "Export"}
				);

				routes.MapRoute(
					name: "areaStats",
    				template: "{area:exists}/stats",
					defaults: new { area = "Admin", controller = "Home", action = "Stats"}
				);
				
				routes.MapRoute(
					name: "areaRoute",
    				template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
				);

				routes.MapRoute(
					name: "search",
					template: "search",
					defaults: new { controller = "Search", action = "Search"}
				);

				routes.MapRoute(
					name: "searchTitle",
					template: "search/title",
					defaults: new { controller = "Search", action = "SearchTitle"}
				);

				routes.MapRoute(
					name: "searchCategory",
					template: "search/category",
					defaults: new { controller = "Search", action = "SearchCategory"}
				);

				routes.MapRoute(
					name: "searchTag",
					template: "search/tag",
					defaults: new { controller = "Search", action = "SearchTag"}
				);

				routes.MapRoute(
					name: "view",
					template: "book/{id?}",
					defaults: new { controller = "Book", action = "View"}
				);

				routes.MapRoute(
					name: "create",
					template: "create",
					defaults: new { controller = "Book", action = "Create"}
				);

				routes.MapRoute(
					name: "tags",
					template: "tags/{ofs?}",
					defaults: new { controller = "Tag", action = "Index" }
				);

				routes.MapRoute(
					name: "tag",
					template: "tag/{id?}/{ofs?}",
					defaults: new { controller = "Tag", action = "View" }
				);

				routes.MapRoute(
					name: "categories",
					template: "categories/{ofs?}",
					defaults: new { controller = "Category", action = "Index" }
				);

				routes.MapRoute(
					name: "category",
					template: "category/{id?}/{ofs?}",
					defaults: new { controller = "Category", action = "View" }
				);

				routes.MapRoute(
					name: "login",
					template: "login",
					defaults: new { controller = "Account", action = "Login"}
				);

				routes.MapRoute(
					name: "logout",
					template: "logout",
					defaults: new { controller = "Account", action = "Logout"}
				);

				routes.MapRoute(
					name: "error",
					template: "error/{code?}",
					defaults: new { controller = "home", action = "error" }
				);

				routes.MapRoute(
					name: "default",
					template: "{controller=Home}/{action=Index}/{id?}"
				);
			});

			return app;
		}
	}
}