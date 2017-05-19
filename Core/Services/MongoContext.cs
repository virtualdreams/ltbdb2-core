using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver.Core.Events;
using MongoDB.Driver;
using ltbdb.Core.Models;

namespace ltbdb.Core.Services
{
	public class MongoContext
	{
		private readonly ILogger<MongoContext> Log;
		private readonly IOptions<Settings> Settings;
		private readonly IMongoClient _client;
		private readonly IMongoDatabase _database;
		public IMongoCollection<Book> Book { get; private set; }

		public MongoContext(IOptions<Settings> settings, ILogger<MongoContext> log)
		{
			Log = log;
			Settings = settings;

			Log.LogDebug("Set mongo database to '{0}'.", Settings.Value.Database);

			var _settings = MongoClientSettings.FromUrl(new MongoUrl(Settings.Value.MongoDB));

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
			_database = _client.GetDatabase(Settings.Value.Database);
			Book = _database.GetCollection<Book>("book");
		}
	}
}