using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using ltbdb.Core.Models;

namespace ltbdb.Core.Services
{
	/// <summary>
	/// Maintenance service class.
	/// </summary>
	public class MaintenanceService
	{
		private readonly ILogger<MaintenanceService> Log;
		private readonly MongoContext Context;

		public MaintenanceService(ILogger<MaintenanceService> log, MongoContext context)
		{
			Log = log;
			Context = context;
		}

		/// <summary>
		/// (Re)Create all necessary indexes for the database.
		/// </summary>
		public void CreateIndexes()
		{
			var _index = Builders<Book>.IndexKeys;
			var _title = _index
				.Ascending(f => f.Title);

			var _category = _index
				.Ascending(f => f.Category);

			var _tags = _index
				.Ascending(f => f.Tags);

			Context.Book.Indexes.DropAll();
			Context.Book.Indexes.CreateOne(_title, new CreateIndexOptions { });
			Context.Book.Indexes.CreateOne(_category, new CreateIndexOptions { });
			//Context.Book.Indexes.CreateOne(_tags, new CreateIndexOptions { Sparse = true });
		}
	}
}