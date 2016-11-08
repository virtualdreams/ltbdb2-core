using ltbdb.Core.Models;
using MongoDB.Bson;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace ltbdb.Core.Services
{
    public class CategoryService : MongoContext
	{
		//private static readonly ILog Log = LogManager.GetLogger(typeof(CategoryService));

		public CategoryService(IMongoClient client)
			: base(client)
		{ }

		/// <summary>
		/// Get all available categories.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> Get()
		{
			return Book.Distinct<string>("Category", new ExpressionFilterDefinition<Book>(_ => true)).ToEnumerable();
		}

		/// <summary>
		/// Rename a category. Returns false if no document modified.
		/// </summary>
		/// <param name="from">The original category name.</param>
		/// <param name="to">The target category name.</param>
		/// <returns></returns>
		public bool Rename(string from, string to)
		{
			from = from.Trim();
			to = to.Trim();
			
			if (String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to))
				return false;
			
			var _filter = Builders<Book>.Filter;
			var _from = _filter.Eq(f => f.Category, from);

			var _update = Builders<Book>.Update;
			var _set = _update.Set(s => s.Category, to);

			var _result = Book.UpdateMany(_from, _set);

			if (_result.IsAcknowledged && _result.ModifiedCount > 0)
			{
				//Log.InfoFormat("Rename category '{0}' to '{1}'. Modified {2} documents.", from, to, _result.ModifiedCount);
				return true;
			}
			else
			{
				//Log.ErrorFormat("Rename category '{0}' failed. No document was modified.", from);
				return false;
			}
		}

		/// <summary>
		/// Get a list of suggestions for term.
		/// </summary>
		/// <param name="term">The term to search for.</param>
		/// <returns></returns>
		public IEnumerable<string> Suggestions(string term)
		{
			term = term.Trim();

			if (String.IsNullOrEmpty(term))
				term = ".*";
			else
				term = Regex.Escape(term);

			var _filter = Builders<Book>.Filter;
			var _category = _filter.Regex(f => f.Category, new BsonRegularExpression(term, "i"));

			var _sort = Builders<Book>.Sort;
			var _order = _sort.Ascending(f => f.Category);

			//if (Log.IsDebugEnabled)
			//{
			//	Log.Debug(Book.Find(_category).Sort(_order).ToString());
			//}

			return Book.Find(_category).Sort(_order).ToEnumerable().Select(s => s.Category).Distinct();
		}
	}
}