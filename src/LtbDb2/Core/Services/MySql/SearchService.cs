using LtbDb.Core.Data;
using LtbDb.Core.Interfaces;
using LtbDb.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace LtbDb.Core.Services.MySql
{
	public class SearchService : ISearchService
	{
		private readonly ILogger<SearchService> Log;

		private readonly DatabaseContext Context;

		public SearchService(
			ILogger<SearchService> log,
			DatabaseContext context)
		{
			Log = log;
			Context = context;
		}

		/// <summary>
		/// Search for books.
		/// </summary>
		/// <param name="term"></param>
		/// <returns></returns>
		public async Task<IList<Book>> SearchAsync(string term)
		{
			term = term.Trim();

			if (String.IsNullOrEmpty(term))
				return new List<Book>();

			Log.LogInformation($"Search for book by term '{term}'.");

			var _query = Context.Book
				.AsNoTracking()
				.Where(f =>
					EF.Functions.Like(f.Title, $"%{term}%") ||
					EF.Functions.Like(f.Number, $"{term}") ||
					f.Stories.Any(a => EF.Functions.Like(a.Name, $"%{term}%")) ||
					f.Tags.Any(a => EF.Functions.Like(a.Name, $"%{term}%"))
				)
				.OrderBy(o => o.Number)
				.ThenBy(o => o.Title);

			return await _query.ToListAsync();
		}

		/// <summary>
		/// Get a list of suggestions for term.
		/// </summary>
		/// <param name="term">The term to search for.</param>
		/// <returns>List of categories.</returns>
		public async Task<IList<string>> SearchSuggestionsAsync(string term)
		{
			term = term.Trim();

			Log.LogDebug($"Request suggestions for books by term '{term}'.");

			var _query = Context.Book
				.AsNoTracking()
				.Where(f =>
					EF.Functions.Like(f.Title, $"%{term}%") ||
					EF.Functions.Like(f.Number, $"{term}") ||
					f.Stories.Any(a => EF.Functions.Like(a.Name, $"%{term}%")) ||
					f.Tags.Any(a => EF.Functions.Like(a.Name, $"%{term}%"))
				)
				.OrderBy(o => o.Title)
				.Select(s => s.Title);

			return await _query.ToListAsync();
		}

		/// <summary>
		/// Get a list of suggestions for term.
		/// </summary>
		/// <param name="term">The term to search for.</param>
		/// <returns></returns>
		public async Task<IList<string>> CategorySuggestionsAsync(string term)
		{
			term = term.Trim();

			var _query = Context.Book
				.AsNoTracking()
				.Where(f => EF.Functions.Like(f.Category, $"%{term}%"))
				.Select(s => s.Category)
				.Distinct()
				.OrderBy(o => 0);

			Log.LogDebug($"Request suggestions for categories by term '{term}'.");

			return await _query.ToListAsync();
		}

		/// <summary>
		/// Get a list of suggestions for term.
		/// </summary>
		/// <param name="term">The term to search for.</param>
		/// <returns>List of tags.</returns>
		public async Task<IList<string>> TagSuggestionsAsync(string term)
		{
			term = term.Trim();

			var _query = Context.Book
				.AsNoTracking()
				.SelectMany(s => s.Tags
					.Where(f => EF.Functions.Like(f.Name, $"%{term}%"))
					.Select(s => s.Name)
				)
				.Distinct()
				.OrderBy(o => o);

			Log.LogDebug($"Request suggestions for tags by term '{term}'.");

			return await _query.ToListAsync();
		}
	}
}