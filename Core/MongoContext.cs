using MongoDB.Driver;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace ltbdb
{
	public class MongoContext
	{
		protected readonly IMongoClient _client;

		protected readonly IMongoDatabase _database;

		protected IMongoCollection<Book> Book { get; private set; }

		public MongoContext(IMongoClient client)
		{
			_client = client;
			_database = _client.GetDatabase("ltbdb");

			Book = _database.GetCollection<Book>("book");
		}
	}

	[BsonIgnoreExtraElements]
	public class Book
	{
		[BsonId]
		[BsonIgnoreIfDefault]
		public ObjectId Id { get; set; }

		public string Title {get; set; }
	}
}