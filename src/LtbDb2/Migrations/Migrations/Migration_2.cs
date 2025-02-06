using FluentMigrator;

namespace LtbDb.Migrations.Migrations
{
	[Migration(2, "Add unique key for number, title and category.")]
	public class Migration_2 : Migration
	{
		public override void Up()
		{
			IfDatabase("postgresql")
				.Execute.EmbeddedScript("Migrations.Schema.Postgres.migration_2.sql");

			IfDatabase("mysql")
				.Execute.EmbeddedScript("Migrations.Schema.MySql.migration_2.sql");
		}

		public override void Down()
		{
			throw new System.NotImplementedException();
		}
	}
}