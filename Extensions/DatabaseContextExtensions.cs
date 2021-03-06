using LtbDb.Core.Data;
using LtbDb.Provider;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace LtbDb.Extensions
{
	public static class DatabaseContextExtensions
	{
		public static IServiceCollection AddDatabaseContext(this IServiceCollection services, string connectionString, DatabaseProvider provider)
		{
			services.AddDbContext<DataContext>(options =>
			{
				switch (provider)
				{
					case DatabaseProvider.MySql:
						options.UseMySql(connectionString, mySqlOptions => { });
						break;

					case DatabaseProvider.PgSql:
						options.UseNpgsql(connectionString, npgSqlOptions => { });
						break;
				}
#if DEBUG
				options.EnableSensitiveDataLogging(true);
#endif
			},
			ServiceLifetime.Scoped);

			return services;
		}
	}
}