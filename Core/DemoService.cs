using System.Collections.Generic;
using MongoDB.Driver;

namespace ltbdb
{
	public class DemoService: MongoContext
	{
		public DemoService(IMongoClient client) 
			: base(client)
		{ }

		public IEnumerable<Book> Get()
		{
			return Book.Find(_ => true).ToEnumerable();
		}
	}
}