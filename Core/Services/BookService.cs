using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;
using ltbdb.Core.Data;
using ltbdb.Core.Interfaces;
using ltbdb.Core.Internal;
using ltbdb.Core.Models;

namespace ltbdb.Core.Services
{
	public class BookService : IBookService
	{
		private readonly ILogger<BookService> Log;
		private readonly DataContext Context;
		private readonly IImageService ImageService;

		/// <summary>
		/// Initializes the BookService class.
		/// </summary>
		/// <param name="log">The logger.</param>
		/// <param name="context">The MySQL context.</param>
		/// <param name="image">The image service.</param>
		public BookService(ILogger<BookService> log, DataContext context, IImageService image)
		{
			Log = log;
			Context = context;
			ImageService = image;
		}

		/// <summary>
		/// Get all books from storage.
		/// </summary>
		/// <returns></returns>
		public async Task<List<Book>> GetAsync()
		{
			Log.LogInformation($"Request all books.");

			var _query = Context.Book
				.AsNoTracking()
				.OrderBy(o => o.Category)
				.ThenBy(o => o.Number);

			return await _query.ToListAsync();
		}

		/// <summary>
		/// Get book by id.
		/// </summary>
		/// <param name="id">The book id.</param>
		/// <returns></returns>
		public async Task<Book> GetByIdAsync(int id)
		{
			Log.LogInformation($"Request book by id {id}.");

			var _query = Context.Book
				.Include(i => i.Stories)
				.Include(i => i.Tags)
				.Where(f => f.Id == id);

			return await _query.SingleOrDefaultAsync();
		}

		/// <summary>
		/// Get books by category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		public async Task<List<Book>> GetByCategoryAsync(string category)
		{
			category = category.Trim();

			Log.LogInformation($"Request books by category '{category}'.");

			var _query = Context.Book
				.AsNoTracking()
				.Where(f => f.Category == category)
				.OrderBy(o => o.Number);

			return await _query.ToListAsync();
		}

		/// <summary>
		/// Get books by tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <returns></returns>
		public async Task<List<Book>> GetByTagAsync(string tag)
		{
			tag = tag.Trim();

			Log.LogInformation($"Request books by tag '{tag}'.");

			var _query = Context.Book
				.AsNoTracking()
				.Where(f => f.Tags.Any(a => a.Name == tag))
				.OrderBy(o => o.Number)
				.ThenBy(o => o.Category);

			return await _query.ToListAsync();
		}

		/// <summary>
		/// Get books by filter. Filter can be category and/or tags.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <param name="tag">The tag.</param>
		/// <returns>List of book.</returns>
		public async Task<List<Book>> GetByFilterAsync(string category, string tag)
		{
			category = category.Trim();
			tag = tag.Trim();

			Log.LogInformation($"Request all books by filter. Filter: category: '{category}', tag: '{tag}'.");

			var _query = Context.Book
				.AsNoTracking()
				.Include(i => i.Stories)
				.Include(i => i.Tags)
				.AsQueryable();

			if (!String.IsNullOrEmpty(category))
				_query = _query.Where(f => f.Category == category);

			if (!String.IsNullOrEmpty(tag))
				_query = _query.Where(f => f.Tags.Any(a => a.Name == tag));

			_query = _query
				.OrderBy(o => o.Id);

			return await _query.ToListAsync();
		}

		/// <summary>
		/// Get recently added books.
		/// </summary>
		/// <param name="limit">Limit result.</param>
		/// <returns></returns>
		public async Task<List<Book>> GetRecentlyAddedAsync(int limit)
		{
			Log.LogInformation($"Request recently added book. (limit {limit})");

			var _query = Context.Book
				.AsNoTracking()
				.OrderByDescending(o => o.Created)
				.Take(limit);

			return await _query.ToListAsync();
		}

		/// <summary>
		/// Search for books.
		/// </summary>
		/// <param name="term"></param>
		/// <returns></returns>
		public async Task<List<Book>> SearchAsync(string term)
		{
			term = term.Trim();

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
		public async Task<List<string>> SuggestionsAsync(string term)
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
		/// Create a new book.
		/// </summary>
		/// <param name="book">The book.</param>
		public async Task<Book> CreateAsync(Book book)
		{
			// remove duplicate entries 
			var _tags = book.Tags
				.Distinct(d => d.Name)
				.ToList();

			var _currentDate = DateTime.Now;

			var _book = new Book
			{
				Number = book.Number,
				Title = book.Title,
				Category = book.Category,
				Created = _currentDate,
				Modified = _currentDate,
				Filename = null,
				Stories = book.Stories,
				Tags = _tags
			};

			Context.Add(_book);
			await Context.SaveChangesAsync();

			Log.LogInformation($"Create new book with id {_book.Id}.");

			return _book;
		}

		/// <summary>
		/// Update an existing book.
		/// </summary>
		/// <param name="book">The book.</param>
		public async Task UpdateAsync(Book book)
		{
			var _book = await GetByIdAsync(book.Id);
			if (_book == null)
				throw new LtbdbNotFoundException();

			// remove duplicate entries 
			var _tags = book.Tags
				.Distinct(d => d.Name)
				.ToList();

			var _currentDate = DateTime.Now;

			_book.Number = book.Number;
			_book.Title = book.Title;
			_book.Category = book.Category;
			_book.Modified = _currentDate;

			var _storiesEqual = _book.Stories.Select(s => s.Name).SequenceEqual(book.Stories.Select(s => s.Name));
			var _tagsEqual = _book.Tags.Select(s => s.Name).SequenceEqual(_tags.Select(s => s.Name));

			Log.LogInformation($"Stories update needed: {!_storiesEqual}");
			Log.LogInformation($"Tags update needed: {!_tagsEqual}");

			if (!_storiesEqual)
			{
				_book.Stories.Clear();
				_book.Stories = book.Stories;
			}

			if (!_tagsEqual)
			{
				_book.Tags.Clear();
				_book.Tags = _tags;
			}

			await Context.SaveChangesAsync();

			Log.LogInformation($"Update book {_book.Id}.");
		}

		/// <summary>
		/// Delete an existing book.
		/// </summary>
		/// <param name="id"></param>
		public async Task DeleteAsync(int id)
		{
			var _book = await GetByIdAsync(id);
			if (_book == null)
				return;

			ImageService.Remove(_book.Filename, true);

			_book.Stories.Clear();
			_book.Tags.Clear();

			Context.Book.Remove(_book);

			await Context.SaveChangesAsync();

			Log.LogInformation($"Delete book {id}.");
		}

		/// <summary>
		/// Set the cover for the book. If stream is null, the image get removed.
		/// </summary>
		/// <param name="id">The id of the book.</param>
		/// <param name="stream">The image stream.</param>
		/// <returns>True on success.</returns>
		public async Task SetImageAsync(int id, Stream stream)
		{
			var _book = await GetByIdAsync(id);

			if (_book == null)
				return;

			ImageService.Remove(_book.Filename);
			if (stream == null)
			{
				// remove the old images
				_book.Filename = null;
			}
			else
			{
				// remove the old image and store the new one
				var _filename = ImageService.Save(stream, true);
				if (String.IsNullOrEmpty(_filename))
					throw new LtbdbInvalidFilenameException();

				_book.Filename = _filename;
			}

			await Context.SaveChangesAsync();

			Log.LogInformation($"Update image for book {id} set filename '{_book.Filename}'.");
		}
	}
}
