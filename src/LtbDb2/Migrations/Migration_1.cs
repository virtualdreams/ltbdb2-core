using FluentMigrator;

namespace LtbDb.Migrations
{
	[Migration(1, "Create database.")]
	public class Migration_1 : Migration
	{
		public override void Up()
		{
			IfDatabase("postgresql")
				.Execute.EmbeddedScript("Migrations.Schema.Postgres.migration_1.sql");

			IfDatabase("mysql")
				.Execute.EmbeddedScript("Migrations.Schema.MySql.migration_1.sql");
		}

		public override void Down()
		{
			throw new System.NotImplementedException();
		}
	}
}