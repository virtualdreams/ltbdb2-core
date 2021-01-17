using LtbDb.Core.Interfaces;
using LtbDb.Core.Services;
using LtbDb.Provider;
using Microsoft.Extensions.DependencyInjection;

namespace LtbDb.Core
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddLtbdbServices(this IServiceCollection services, DatabaseProvider provider)
		{
			services.AddTransient<IBookService, BookService>();
			services.AddTransient<ITagService, TagService>();
			services.AddTransient<ICategoryService, CategoryService>();
			services.AddTransient<IMaintenanceService, MaintenanceService>();
			services.AddTransient<IImageService, ImageService>();
			services.AddSearchService(provider);

			return services;
		}

		private static IServiceCollection AddSearchService(this IServiceCollection services, DatabaseProvider provider)
		{
			switch (provider)
			{
				case DatabaseProvider.MySql:
					services.AddTransient<ISearchService, Core.Services.MySql.SearchService>();
					break;

				case DatabaseProvider.PgSql:
					services.AddTransient<ISearchService, Core.Services.PgSql.SearchService>();
					break;
			}

			return services;
		}
	}
}