using Microsoft.AspNetCore.Builder;

namespace ltbdb.Extensions
{
	static public class RoutingExtensions
	{
		static public IApplicationBuilder AddRoutes(this IApplicationBuilder app)
		{
			app.UseMvc(routes => {
				routes.MapRoute(
					name: "areaRoute",
    				template: "{area:exists}/{controller=Home}/{action=Index}/{id?}"
				);

				routes.MapRoute(
					name: "Search",
					template: "search",
					defaults: new { controller = "Home", action = "search"}
				);

				routes.MapRoute(
					name: "Book",
					template: "book/{id?}/{slug?}",
					defaults: new { controller = "Book", action = "View"},
					constraints: new { id = @"^[a-f0-9]{24}$" }
				);

				routes.MapRoute(
					name: "Tags",
					template: "tags",
					defaults: new { controller = "tag", action = "index" }
				);

				routes.MapRoute(
					name: "Tag",
					template: "tag/{id?}",
					defaults: new { controller = "tag", action = "view" }
				);

				routes.MapRoute(
					name: "Categories",
					template: "categories",
					defaults: new { controller = "category", action = "index" }
				);

				routes.MapRoute(
					name: "Category",
					template: "category/{id?}",
					defaults: new { controller = "category", action = "view" }
				);

				routes.MapRoute(
					name: "Login",
					template: "login",
					defaults: new { controller = "account", action = "login"}
				);

				routes.MapRoute(
					name: "Logout",
					template: "logout",
					defaults: new { controller = "account", action = "logout"}
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