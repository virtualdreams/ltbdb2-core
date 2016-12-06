using ltbdb.Core.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace ltbdb.Core.Services
{
    public class MongoContext
	{
		private readonly IOptions<Settings> Settings;
		protected readonly IMongoClient _client;
		protected readonly IMongoDatabase _db;
		public IMongoCollection<Book> Book { get; private set; }
		public IMongoCollection<User> User { get; private set; }
		//protected GridFSBucket Images { get; private set; }
		//protected GridFSBucket Thumbs { get; private set; }

		public MongoContext(IOptions<Settings> settings)
		{
			Settings = settings;

			_client = new MongoClient(Settings.Value.MongoDB);
			_db = _client.GetDatabase(Settings.Value.Database);
			Book = _db.GetCollection<Book>("book");
			User = _db.GetCollection<User>("user");
			//Images = new GridFSBucket(_db, new GridFSBucketOptions { BucketName = "images" });
			//Thumbs = new GridFSBucket(_db, new GridFSBucketOptions { BucketName = "thumbs" });
		}
	}
}