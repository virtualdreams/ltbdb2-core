using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using ltbdb.Core.Data;

namespace ltbdb.Extensions
{
	public static class DatabaseContextExtensions
	{
		public static IServiceCollection AddDatabaseContext(this IServiceCollection services, string connectionString)
		{
			services.AddDbContext<DataContext>(options =>
			{
				options.UseMySql(connectionString, mySqlOptions => { });
#if DEBUG
				options.EnableSensitiveDataLogging(true);
#endif
			},
			ServiceLifetime.Scoped);

			return services;
		}
	}
}