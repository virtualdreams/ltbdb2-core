using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Core.Events;
using ltbdb.Core.Models;

namespace ltbdb.Core.Services
{
	public class MongoContext
	{
		private readonly ILogger<MongoContext> Log;
		private readonly Settings Options;
		private readonly IMongoClient _client;
		private readonly IMongoDatabase _database;
		public IMongoCollection<Book> Book { get; private set; }

		public MongoContext(Settings settings, ILogger<MongoContext> log)
		{
			Log = log;
			Options = settings;

			Log.LogDebug("Set mongo database to '{0}'.", Options.Database);

			var _settings = MongoClientSettings.FromUrl(new MongoUrl(Options.MongoDB));

			if (Log.IsEnabled(LogLevel.Debug))
			{
				Log.LogTrace("MongoDB command log enabled.");
				_settings.ClusterConfigurator = cb =>
				{
					cb.Subscribe<CommandStartedEvent>(e =>
					{
						Log.LogTrace($"{e.CommandName} - {e.Command.ToJson()}");
					});
				};
			}

			_client = new MongoClient(_settings);
			_database = _client.GetDatabase(Options.Database);
			Book = _database.GetCollection<Book>("book");
		}
	}
}