using ltbdb.Core.Models;
using MongoDB.Bson;
using MongoDB.Bson.Serialization;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Microsoft.Extensions.Logging;

namespace ltbdb.Core.Services
{
    public class TagService
	{
		private readonly ILogger<TagService> Log;
		private readonly MongoContext Context;

		private class Tag
		{
			public string _id { get; set; }
		}

		public TagService(ILogger<TagService> logger, MongoContext context)
		{
			Log = logger;
			Context = context;
		}

		/// <summary>
		/// Get all available tags.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> Get()
		{
			return Context.Book.Distinct<string>("Tags", new ExpressionFilterDefinition<Book>(_ => true)).ToEnumerable();
		}

		/// <summary>
		/// Get a list of suggestions for term.
		/// </summary>
		/// <param name="term">The term to search for.</param>
		/// <returns>List of tags.</returns>
		public IEnumerable<string> Suggestions(string term)
		{
			term = term.Trim();

			return Context.Book.Aggregate()
				.Unwind("Tags")
				.Match(new BsonDocument { { "Tags", new BsonRegularExpression(Regex.Escape(term), "i") } })
				.Group(new BsonDocument { { "_id", "$Tags" } })
				.ToEnumerable()
				.Select(s => BsonSerializer.Deserialize<Tag>(s))
				.Select(s => s._id);
		}
	}
}