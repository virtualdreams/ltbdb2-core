using Microsoft.Extensions.DependencyInjection;
using ltbdb.Core.Interfaces;
using ltbdb.Core.Services;

namespace ltbdb.Core
{
	public static class ServiceCollectionExtensions
	{
		public static IServiceCollection AddLtbdbServices(this IServiceCollection services)
		{
			services.AddTransient<IBookService, BookService>();
			services.AddTransient<ITagService, TagService>();
			services.AddTransient<ICategoryService, CategoryService>();
			services.AddTransient<IMaintenanceService, MaintenanceService>();
			services.AddTransient<IImageService, ImageService>();

			return services;
		}
	}
}