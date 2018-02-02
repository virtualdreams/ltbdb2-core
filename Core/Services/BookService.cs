using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System;
using ltbdb.Core.Models;

namespace ltbdb.Core.Services
{
	public class BookService
	{
		private readonly ILogger<BookService> Log;
		private readonly MongoContext Context;
		private readonly ImageService ImageService;

		/// <summary>
		/// Initializes the BookService class.
		/// </summary>
		/// <param name="client">The mongo client.</param>
		/// <param name="logger">The logger.</param>
		public BookService(ILogger<BookService> logger, MongoContext context, ImageService image)
		{
			Log = logger;
			Context = context;
			ImageService = image;
		}

		/// <summary>
		/// Get all books from storage.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Book> Get()
		{
			var _filter = Builders<Book>.Filter;
			var _all = _filter.Empty;

			var _sort = Builders<Book>.Sort;
			var _order = _sort.Ascending(o => o.Number).Ascending(o => o.Category);

			Log.LogInformation($"Request all books.");

			var _result = Context.Book
				.Find(_all)
				.Sort(_order)
				.ToEnumerable();

			return _result;
		}

		/// <summary>
		/// Get book by id.
		/// </summary>
		/// <param name="id"></param>
		/// <returns></returns>
		public Book GetById(ObjectId id)
		{
			var _filter = Builders<Book>.Filter;
			var _id = _filter.Eq(f => f.Id, id);

			Log.LogInformation($"Request book by id '{id.ToString()}'.");

			var _result = Context.Book
				.Find(_id)
				.SingleOrDefault();

			return _result;
		}

		/// <summary>
		/// Get books by category.
		/// </summary>
		/// <param name="category"></param>
		/// <returns></returns>
		public IEnumerable<Book> GetByCategory(string category)
		{
			category = category.Trim();

			var _filter = Builders<Book>.Filter;
			var _category = _filter.Eq(f => f.Category, category);

			var _sort = Builders<Book>.Sort;
			var _order = _sort.Ascending(f => f.Number);

			Log.LogInformation($"Request books by category '{category}'.");

			var _result = Context.Book
				.Find(_category)
				.Sort(_order)
				.ToEnumerable();

			return _result;
		}

		/// <summary>
		/// Get books by tag.
		/// </summary>
		/// <param name="tag"></param>
		/// <returns></returns>
		public IEnumerable<Book> GetByTag(string tag)
		{
			tag = tag.Trim();

			var _filter = Builders<Book>.Filter;
			var _tag = _filter.AnyIn("Tags", new string[] { tag });

			var _sort = Builders<Book>.Sort;
			var _order = _sort.Ascending(f => f.Number).Ascending(f => f.Category);

			Log.LogInformation($"Request books by tag '{tag}'.");

			var _result = Context.Book
				.Find(_tag)
				.Sort(_order)
				.ToEnumerable();

			return _result;
		}

		/// <summary>
		/// Get recently added books.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<Book> GetRecentlyAdded(int limit)
		{
			var _filter = Builders<Book>.Filter;
			var _all = _filter.Empty;

			var _sort = Builders<Book>.Sort;
			var _order = _sort.Descending(o => o.Created);

			Log.LogInformation($"Request recently added book. (limit {limit})");

			var _result = Context.Book
				.Find(_all)
				.Sort(_order)
				.Limit(limit)
				.ToEnumerable();

			return _result;
		}

		/// <summary>
		/// Export the complete database.
		/// </summary>
		/// <returns></returns>
		public IEnumerable<dynamic> Export()
		{
			foreach (var book in Get().OrderBy(o => o.Category).ThenBy(o => o.Number))
			{
				yield return new
				{
					Number = book.Number,
					Title = book.Title,
					Category = book.Category,
					Created = book.Created,
					Filename = book.Filename,
					Stories = book.Stories,
					Tags = book.Tags
				};
			}
		}

		/// <summary>
		/// Search for books.
		/// </summary>
		/// <param name="term"></param>
		/// <returns></returns>
		public IEnumerable<Book> Search(string term)
		{
			term = term.Trim();

			var _filter = Builders<Book>.Filter;
			var _title = _filter.Regex(f => f.Title, new BsonRegularExpression(Regex.Escape(term), "i"));
			var _stories = _filter.Regex("Stories", new BsonRegularExpression(Regex.Escape(term), "i"));

			var _query = _title | _stories;

			var _n = 0;
			if (Int32.TryParse(term, out _n))
			{
				var _number = _filter.Eq(f => f.Number, _n);
				_query |= _number;
			}

			var _sort = Builders<Book>.Sort;
			var _order = _sort.Ascending(f => f.Number).Ascending(f => f.Title);

			Log.LogInformation($"Search for book by term '{term}'.");

			var _result = Context.Book
				.Find(_query)
				.Sort(_order)
				.ToEnumerable();

			return _result;
		}

		/// <summary>
		/// Get a list of suggestions for term.
		/// </summary>
		/// <param name="term">The term to search for.</param>
		/// <returns>List of categories.</returns>
		public IEnumerable<string> Suggestions(string term)
		{
			term = term.Trim();

			var _filter = Builders<Book>.Filter;
			var _title = _filter.Regex(f => f.Title, new BsonRegularExpression(Regex.Escape(term), "i"));

			var _sort = Builders<Book>.Sort;
			var _order = _sort.Ascending(f => f.Title);

			Log.LogDebug($"Request suggestions for books by term '{term}'.");

			var _result = Context.Book
				.Find(_title)
				.Sort(_order)
				.ToEnumerable()
				.Select(s => s.Title);

			return _result;
		}

		/// <summary>
		/// Create a new book.
		/// </summary>
		/// <param name="book"></param>
		public ObjectId Create(Book book)
		{
			book.Filename = null;
			book.Created = DateTime.Now;

			Context.Book.InsertOne(book);

			Log.LogInformation($"Create new book with id '{book.Id}'.");

			return book.Id;
		}

		/// <summary>
		/// Update an existing book.
		/// </summary>
		/// <param name="book"></param>
		public void Update(Book book)
		{
			var _filter = Builders<Book>.Filter;
			var _id = _filter.Eq(f => f.Id, book.Id);

			var _update = Builders<Book>.Update;
			var _set = _update
				.Set(s => s.Number, book.Number)
				.Set(s => s.Title, book.Title)
				.Set(s => s.Category, book.Category)
				.Set(s => s.Stories, book.Stories)
				.Set(s => s.Tags, book.Tags);

			Log.LogInformation($"Update book '{book.Id}'.");

			Context.Book.UpdateOne(_id, _set);
		}

		/// <summary>
		/// Delete an existing book.
		/// </summary>
		/// <param name="id"></param>
		public void Delete(ObjectId id)
		{
			var _book = GetById(id);

			if (_book == null)
				return;

			var _filter = Builders<Book>.Filter;
			var _id = _filter.Eq(f => f.Id, _book.Id);

			Log.LogInformation($"Delete book '{id.ToString()}'.");

			Context.Book.DeleteOne(_id);
			RemoveImage(_book.Filename);
		}

		/// <summary>
		/// Set the cover for the book. If stream is null, the image get removed.
		/// </summary>
		/// <param name="id">The id of the book.</param>
		/// <param name="stream">The image stream.</param>
		/// <returns>True on success.</returns>
		public void SetImage(ObjectId id, Stream stream)
		{
			var _book = GetById(id);

			if (_book == null)
				return;

			RemoveImage(_book.Filename);
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

			var _filter = Builders<Book>.Filter;
			var _id = _filter.Eq(f => f.Id, id);

			var _update = Builders<Book>.Update;
			var _set = _update.Set(f => f.Filename, _book.Filename);

			Log.LogInformation($"Update image for book '{id.ToString()}'");

			Context.Book.UpdateOne(_id, _set);
		}

		/// <summary>
		/// Remove image from storage.
		/// </summary>
		/// <param name="filename">The filename.</param>
		private void RemoveImage(string filename)
		{
			ImageService.Remove(filename, true);
		}
	}
}