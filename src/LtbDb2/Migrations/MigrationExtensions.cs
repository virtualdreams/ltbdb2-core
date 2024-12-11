using FluentMigrator.Runner;
using LtbDb.Provider;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using System.Reflection;

namespace LtbDb.Migrations
{
	public static class MigrationExtensions
	{
		public static IServiceCollection AddFluentMigrator(this IServiceCollection services, string connectionString, DatabaseProvider provider)
		{
			services.AddFluentMigratorCore();
			services.ConfigureRunner(config =>
			{
				switch (provider)
				{
					case DatabaseProvider.PgSql:
						config
							.AddPostgres()
							.WithVersionTable(new VersionInfo())
							.WithGlobalConnectionString(connectionString)
							.ScanIn(Assembly.GetExecutingAssembly())
								.For.Migrations()
								.For.EmbeddedResources();
						break;

					case DatabaseProvider.MySql:
						config
							.AddMySql8()
							.WithVersionTable(new VersionInfo())
							.WithGlobalConnectionString(connectionString)
							.ScanIn(Assembly.GetExecutingAssembly())
								.For.Migrations()
								.For.EmbeddedResources();
						break;

				}
			});

			return services;
		}

		public static IApplicationBuilder MigrateDatabase(this IApplicationBuilder app)
		{
			using (var scope = app.ApplicationServices.CreateScope())
			{
				var _runner = scope.ServiceProvider.GetService<IMigrationRunner>();
				_runner.ListMigrations();
				_runner.MigrateUp();
			}
			return app;
		}
	}
}