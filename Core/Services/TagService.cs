using ltbdb.Core.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ltbdb.Core.Services
{
    public class TagService: MongoContext
	{
		//private static readonly ILog Log = LogManager.GetLogger(typeof(TagService));

		private class Tag
		{
			public string _id { get; set; }
		}

		public TagService(IMongoClient client)
			: base(client)
		{ }

		/// <summary>
		/// Get all available tags.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> Get()
		{
			return Book.Distinct<string>("Tags", new ExpressionFilterDefinition<Book>(_ => true)).ToEnumerable();
			
			//return Book.Find(_ => true).ToEnumerable().SelectMany(s => s.Tags).Distinct();
		}

		/// <summary>
		/// Get a list of suggestions for term.
		/// </summary>
		/// <param name="term">The term to search for.</param>
		/// <returns>List of tags.</returns>
		public IEnumerable<string> Suggestions(string term)
		{
			term = term.Trim();

			return Book.Aggregate()
				.Unwind("Tags")
				.Match(new BsonDocument { { "Tags", new BsonRegularExpression(Regex.Escape(term), "i") } })
				.Group(new BsonDocument { { "_id", "$Tags" } })
				.ToEnumerable()
				.Select(s => BsonSerializer.Deserialize<Tag>(s))
				.Select(s => s._id);
		}
	}
}