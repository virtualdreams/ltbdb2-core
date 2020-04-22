using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System;
using ltbdb.Core.Models;

namespace ltbdb.Core.Services
{
	public class BookService
	{
		private readonly ILogger<BookService> Log;
		private readonly MySqlContext Context;
		private readonly ImageService ImageService;

		/// <summary>
		/// Initializes the BookService class.
		/// </summary>
		/// <param name="log">The logger.</param>
		/// <param name="context">The MySQL context.</param>
		/// <param name="image">The image service.</param>
		public BookService(ILogger<BookService> log, MySqlContext context, ImageService image)
		{
			Log = log;
			Context = context;
			ImageService = image;
		}

		/// <summary>
		/// Get all books from storage.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Book> Get()
		{
			Log.LogInformation($"Request all books.");

			var _query = Context.Book
				.AsNoTracking()
				.OrderBy(o => o.Category)
				.ThenBy(o => o.Number);

			return _query;
		}

		/// <summary>
		/// Get book by id.
		/// </summary>
		/// <param name="id">The book id.</param>
		/// <returns></returns>
		public Book GetById(int id)
		{
			Log.LogInformation($"Request book by id {id}.");

			var _query = Context.Book
				.Include(i => i.Stories)
				.Include(i => i.Tags)
				.Where(f => f.Id == id);

			return _query.SingleOrDefault();
		}

		/// <summary>
		/// Get books by category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		public IEnumerable<Book> GetByCategory(string category)
		{
			category = category.Trim();

			Log.LogInformation($"Request books by category '{category}'.");

			var _query = Context.Book
				.AsNoTracking()
				.Where(f => f.Category == category)
				.OrderBy(o => o.Number);

			return _query;
		}

		/// <summary>
		/// Get books by tag.
		/// </summary>
		/// <param name="tag">The tag.</param>
		/// <returns></returns>
		public IEnumerable<Book> GetByTag(string tag)
		{
			tag = tag.Trim();

			Log.LogInformation($"Request books by tag '{tag}'.");

			var _query = Context.Book
				.AsNoTracking()
				.Where(f => f.Tags.Any(a => a.Name == tag))
				.OrderBy(o => o.Number)
				.ThenBy(o => o.Category);

			return _query;
		}

		/// <summary>
		/// Get books by filter. Filter can be category and/or tags.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <param name="tag">The tag.</param>
		/// <returns>List of book.</returns>
		public IEnumerable<Book> GetByFilter(string category, string tag)
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

			return _query;
		}

		/// <summary>
		/// Get recently added books.
		/// </summary>
		/// <param name="limit">Limit result.</param>
		/// <returns></returns>
		public IEnumerable<Book> GetRecentlyAdded(int limit)
		{
			Log.LogInformation($"Request recently added book. (limit {limit})");

			var _query = Context.Book
				.AsNoTracking()
				.OrderByDescending(o => o.Created)
				.Take(limit);

			return _query;
		}

		/// <summary>
		/// Search for books.
		/// </summary>
		/// <param name="term"></param>
		/// <returns></returns>
		public IEnumerable<Book> Search(string term)
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

			return _query;
		}

		/// <summary>
		/// Get a list of suggestions for term.
		/// </summary>
		/// <param name="term">The term to search for.</param>
		/// <returns>List of categories.</returns>
		public IEnumerable<string> Suggestions(string term)
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

			return _query;
		}

		/// <summary>
		/// Create a new book.
		/// </summary>
		/// <param name="book">The book.</param>
		public Book Create(Book book)
		{
			book.Filename = null;
			book.Created = DateTime.Now;

			Context.Add(book);
			Context.SaveChanges();

			Log.LogInformation($"Create new book with id {book.Id}.");

			return book;
		}

		/// <summary>
		/// Update an existing book.
		/// </summary>
		/// <param name="book">The book.</param>
		public void Update(Book book)
		{
			var _book = GetById(book.Id);
			if (_book == null)
				throw new LtbdbNotFoundException();

			_book.Number = book.Number;
			_book.Title = book.Title;
			_book.Category = book.Category;

			var _storiesEqual = _book.Stories.Select(s => s.Name).SequenceEqual(book.Stories.Select(s => s.Name));
			var _tagsEqual = _book.Tags.Select(s => s.Name).SequenceEqual(book.Tags.Select(s => s.Name));
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
				_book.Tags = book.Tags;
			}

			Context.SaveChanges();

			Log.LogInformation($"Update book {book.Id}.");
		}

		/// <summary>
		/// Delete an existing book.
		/// </summary>
		/// <param name="id"></param>
		public void Delete(int id)
		{
			var _book = GetById(id);
			if (_book == null)
				return;

			ImageService.Remove(_book.Filename, true);

			_book.Stories.Clear();
			_book.Tags.Clear();

			Context.Book.Remove(_book);

			Log.LogInformation($"Delete book {id}.");

			Context.SaveChanges();
		}

		/// <summary>
		/// Set the cover for the book. If stream is null, the image get removed.
		/// </summary>
		/// <param name="id">The id of the book.</param>
		/// <param name="stream">The image stream.</param>
		/// <returns>True on success.</returns>
		public void SetImage(int id, Stream stream)
		{
			var _book = GetById(id);

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

			Log.LogInformation($"Update image for book {id} set filename '{_book.Filename}'.");

			Context.SaveChanges();
		}
	}
}
