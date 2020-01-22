using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

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
		public dynamic Stats()
		{
			var _books = Context.Book.Count();
			var _categories = Context.Book.Select(s => s.Category).Distinct().Count();
			var _stories = Context.Book.SelectMany(s => s.Stories).Count();
			var _tags = Context.Tag.Select(s => s.Name).Distinct().Count();

			return new
			{
				Books = _books,
				Categories = _categories,
				Stories = _stories,
				Tags = _tags
			};
		}
	}
}