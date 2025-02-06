using LtbDb.Core.Data;
using LtbDb.Core.Extensions;
using LtbDb.Core.Interfaces;
using LtbDb.Core.Internal;
using LtbDb.Core.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using MySqlConnector;
using Npgsql;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace LtbDb.Core.Services
{
	public class BookService : IBookService
	{
		private readonly ILogger<BookService> Log;

		private readonly DatabaseContext Context;

		private readonly IImageService ImageService;

		/// <summary>
		/// Initializes the BookService class.
		/// </summary>
		/// <param name="log">The logger.</param>
		/// <param name="context">The MySQL context.</param>
		/// <param name="image">The image service.</param>
		public BookService(
			ILogger<BookService> log,
			DatabaseContext context,
			IImageService image)
		{
			Log = log;
			Context = context;
			ImageService = image;
		}

		/// <summary>
		/// Get all books from storage.
		/// </summary>
		/// <returns></returns>
		public async Task<IList<Book>> GetAsync()
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
				.Include(i => i.Stories
					.OrderBy(o => o.ItemOrder))
				.Include(i => i.Tags)
				.Where(f => f.Id == id)
				.AsSplitQuery();

			return await _query.SingleOrDefaultAsync();
		}

		/// <summary>
		/// Get books by category.
		/// </summary>
		/// <param name="category">The category.</param>
		/// <returns></returns>
		public async Task<IList<Book>> GetByCategoryAsync(string category)
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
		public async Task<IList<Book>> GetByTagAsync(string tag)
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
		public async Task<IList<Book>> GetByFilterAsync(string category, string tag)
		{
			category = category.Trim();
			tag = tag.Trim();

			Log.LogInformation($"Request all books by filter. Filter: category: '{category}', tag: '{tag}'.");

			var _query = Context.Book
				.AsNoTracking()
				.Include(i => i.Stories
					.OrderBy(o => o.ItemOrder))
				.Include(i => i.Tags)
				.WhereIf(!String.IsNullOrEmpty(category), f => f.Category == category)
				.WhereIf(!String.IsNullOrEmpty(tag), f => f.Tags.Any(a => a.Name == tag))
				.OrderBy(o => o.Id)
				.AsSplitQuery();

			return await _query.ToListAsync();
		}

		/// <summary>
		/// Get recently added books.
		/// </summary>
		/// <param name="limit">Limit result.</param>
		/// <returns></returns>
		public async Task<IList<Book>> GetRecentlyAddedAsync(int limit)
		{
			Log.LogInformation($"Request recently added book. (limit {limit})");

			var _query = Context.Book
				.AsNoTracking()
				.OrderByDescending(o => o.Created)
				.Take(limit);

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

			// assign item order
			var _itemOrder = 1;
			var _stories = book.Stories
				.Select(s => new Story { Name = s.Name, ItemOrder = _itemOrder++ })
				.ToList();

			var _currentDate = DateTime.UtcNow;

			var _book = new Book
			{
				Number = book.Number,
				Title = book.Title,
				Category = book.Category,
				Created = _currentDate,
				Modified = _currentDate,
				Filename = null,
				Stories = _stories,
				Tags = _tags
			};

			Context.Add(_book);

			try
			{
				await Context.SaveChangesAsync();
			}
			catch (DbUpdateException e)
			{
				var p = e.InnerException as PostgresException;
				if (p != null && p.SqlState == "23505")
				{
					Log.LogInformation("Duplicate book entry not allowed.");
					throw new LtbdbDuplicateEntryException();
				}

				var m = e.InnerException as MySqlException;
				if (m != null && m.Number == 1062)
				{
					Log.LogInformation("Duplicate book entry not allowed.");
					throw new LtbdbDuplicateEntryException();
				}
			}

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

			// assign item order
			var _itemOrder = 1;
			var _stories = book.Stories
				.Select(s => new Story { Name = s.Name, ItemOrder = _itemOrder++ })
				.ToList();

			var _currentDate = DateTime.UtcNow;

			_book.Number = book.Number;
			_book.Title = book.Title;
			_book.Category = book.Category;
			_book.Modified = _currentDate;

			var _storiesEqual = _book.Stories
				.Select(s => new { s.Name, s.ItemOrder })
				.SequenceEqual(_stories.Select(s => new { s.Name, s.ItemOrder }));

			var _tagsEqual = _book.Tags
				.Select(s => s.Name)
				.SequenceEqual(_tags.Select(s => s.Name));

			Log.LogInformation($"Stories update needed: {!_storiesEqual}");
			Log.LogInformation($"Tags update needed: {!_tagsEqual}");

			if (!_storiesEqual)
			{
				_book.Stories.Clear();
				_book.Stories = _stories;
			}

			if (!_tagsEqual)
			{
				_book.Tags.Clear();
				_book.Tags = _tags;
			}

			try
			{
				await Context.SaveChangesAsync();
			}
			catch (DbUpdateException e)
			{
				var p = e.InnerException as PostgresException;
				if (p != null && p.SqlState == "23505")
				{
					throw new LtbdbDuplicateEntryException();
				}

				var m = e.InnerException as MySqlException;
				if (m != null && m.Number == 1062)
				{
					throw new LtbdbDuplicateEntryException();
				}
			}

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
