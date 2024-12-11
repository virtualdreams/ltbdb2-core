using FluentMigrator.Runner.VersionTableInfo;

namespace LtbDb.Migrations
{
	[VersionTableMetaData]
	public class VersionInfo : IVersionTableMetaData
	{
		public object ApplicationContext { get; set; }

		public bool OwnsSchema => true;

		public string SchemaName => "public";

		public string TableName => "schema";

		public string ColumnName => "version";

		public string DescriptionColumnName => "description";

		public string UniqueIndexName => "ix_schema_version";

		public string AppliedOnColumnName => "applied_on";

		public bool CreateWithPrimaryKey => true;
	}
}