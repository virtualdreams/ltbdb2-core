using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System;

namespace ltbdb.Core.Services
{
	public class CategoryService
	{
		private readonly ILogger<CategoryService> Log;
		private readonly MySqlContext Context;

		public CategoryService(ILogger<CategoryService> log, MySqlContext context)
		{
			Log = log;
			Context = context;
		}

		/// <summary>
		/// Get all available categories.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> Get()
		{
			Log.LogInformation($"Get the full list of categories.");

			var _query = Context.Book
				.AsNoTracking()
				.GroupBy(g => g.Category)
				.Select(s => s.Key)
				.OrderBy(o => o);

			return _query.ToList();
		}

		/// <summary>
		/// Rename a category. Returns false if no document modified.
		/// </summary>
		/// <param name="from">The original category name.</param>
		/// <param name="to">The target category name.</param>
		/// <returns></returns>
		public void Rename(string from, string to)
		{
			from = from.Trim();
			to = to.Trim();

			if (String.IsNullOrEmpty(from) || String.IsNullOrEmpty(to))
				throw new LtbdbRenameCategoryException();

			var _query = Context.Book
				.Where(f => f.Category == from)
				.ToList();

			_query.ForEach(e =>
			{
				e.Category = to;
			});

			Log.LogInformation($"Rename category '{from}' to '{to}'.");

			Context.SaveChanges();
		}

		/// <summary>
		/// Get a list of suggestions for term.
		/// </summary>
		/// <param name="term">The term to search for.</param>
		/// <returns></returns>
		public IEnumerable<string> Suggestions(string term)
		{
			term = term.Trim();

			var _query = Context.Book
				.AsNoTracking()
				.Where(f => EF.Functions.Like(f.Category, $"%{term}%"))
				.GroupBy(g => g.Category)
				.Select(s => s.Key)
				.OrderBy(o => o);

			Log.LogDebug($"Request suggestions for categories by term '{term}'.");

			return _query.ToList();
		}
	}
}