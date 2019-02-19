using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;

namespace ltbdb.Core.Services
{
	public class MaintenanceService
	{
		private readonly ILogger<MaintenanceService> Log;
		private readonly MySqlContext Context;

		public MaintenanceService(ILogger<MaintenanceService> logger, MySqlContext context)
		{
			Log = logger;
			Context = context;
		}

		/// <summary>
		/// Export the complete database.
		/// </summary>
		/// <returns>List of json ready objects.</returns>
		public IEnumerable<dynamic> Export()
		{
			var _books = Context.Book
				.Include(i => i.Stories)
				.Include(i => i.Tags)
				.OrderBy(o => o.Category)
				.ThenBy(o => o.Number);

			foreach (var book in _books)
			{
				yield return new
				{
					Number = book.Number,
					Title = book.Title,
					Category = book.Category,
					Created = book.Created,
					Filename = book.Filename,
					Stories = book.Stories.Select(s => s.Name),
					Tags = book.Tags.Select(s => s.Name)
				};
			}
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