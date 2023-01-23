using LtbDb.Core.Data;
using LtbDb.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace LtbDb.Core.Services
{
	public class TagService : ITagService
	{
		private readonly ILogger<TagService> Log;

		private readonly DataContext Context;

		public TagService(
			ILogger<TagService> log,
			DataContext context)
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
	}
}