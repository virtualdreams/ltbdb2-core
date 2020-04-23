using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using ltbdb.Core.Models;

namespace ltbdb.Core.Services
{
	public class MaintenanceService
	{
		private readonly ILogger<MaintenanceService> Log;
		private readonly MySqlContext Context;

		public MaintenanceService(ILogger<MaintenanceService> log, MySqlContext context)
		{
			Log = log;
			Context = context;
		}

		/// <summary>
		/// Get database statistics.
		/// </summary>
		/// <returns>List of json ready objects.</returns>
		public async Task<Statistic> GetStatisticsAsync()
		{
			var _books = await Context.Book.CountAsync();
			var _categories = await Context.Book.Select(s => s.Category).Distinct().CountAsync();
			var _stories = await Context.Book.SelectMany(s => s.Stories).CountAsync();
			var _tags = await Context.Tag.Select(s => s.Name).Distinct().CountAsync();

			return new Statistic
			{
				Books = _books,
				Categories = _categories,
				Stories = _stories,
				Tags = _tags
			};
		}
	}
}