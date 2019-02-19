using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using ltbdb.Core.Models;

namespace ltbdb.Core.Services
{
	public class TagService
	{
		private readonly ILogger<TagService> Log;
		private readonly MySqlContext Context;

		public TagService(ILogger<TagService> log, MySqlContext context)
		{
			Log = log;
			Context = context;
		}

		/// <summary>
		/// Get all available tags.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<string> Get()
		{
			Log.LogInformation($"Get the full list of tags.");

			var _query = Context.Tag
				.GroupBy(g => g.Name)
				.Select(s => s.Key)
				.OrderBy(o => o);

			return _query.ToList();
		}

		/// <summary>
		/// Get a list of suggestions for term.
		/// </summary>
		/// <param name="term">The term to search for.</param>
		/// <returns>List of tags.</returns>
		public IEnumerable<string> Suggestions(string term)
		{
			term = term.Trim();

			var _tags = Get()
				.Where(f => f.Contains(term));

			Log.LogDebug($"Request suggestions for tags by term '{term}'.");

			return _tags;
		}
	}
}