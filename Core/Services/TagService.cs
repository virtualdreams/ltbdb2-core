using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ltbdb.Core.Data;
using ltbdb.Core.Interfaces;

namespace ltbdb.Core.Services
{
	public class TagService : ITagService
	{
		private readonly ILogger<TagService> Log;
		private readonly DataContext Context;

		public TagService(ILogger<TagService> log, DataContext context)
		{
			Log = log;
			Context = context;
		}

		/// <summary>
		/// Get all available tags.
		/// </summary>
		/// <returns></returns>
		public async Task<List<string>> GetAsync()
		{
			Log.LogInformation($"Get the full list of tags.");

			var _query = Context.Tag
				.AsNoTracking()
				.GroupBy(g => g.Name)
				.Select(s => s.Key)
				.OrderBy(o => o);

			return await _query.ToListAsync();
		}

		/// <summary>
		/// Get a list of suggestions for term.
		/// </summary>
		/// <param name="term">The term to search for.</param>
		/// <returns>List of tags.</returns>
		public async Task<List<string>> SuggestionsAsync(string term)
		{
			term = term.Trim();

			var _query = Context.Tag
				.AsNoTracking()
				.Where(w => EF.Functions.Like(w.Name, $"%{term}%"))
				.GroupBy(g => g.Name)
				.Select(s => s.Key)
				.OrderBy(o => o);

			Log.LogDebug($"Request suggestions for tags by term '{term}'.");

			return await _query.ToListAsync();
		}
	}
}