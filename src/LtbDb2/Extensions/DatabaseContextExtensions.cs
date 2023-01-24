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
			services.AddDbContext<DatabaseContext>(options =>
			{
				switch (provider)
				{
					case DatabaseProvider.MySql:
						options.UseMySql(connectionString, ServerVersion.AutoDetect(connectionString), mySqlOptions => { });
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