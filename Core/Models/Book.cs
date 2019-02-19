using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ltbdb.Core.Models
{
	/// <summary>
	/// Object to store a book.
	/// </summary>
	[Table("book")]
	public class Book
	{
		/// <summary>
		/// The book id.
		/// </summary>
		[Column("id")]
		public int Id { get; set; }

		/// <summary>
		/// The book number.
		/// </summary>
		[Column("number")]
		public int Number { get; set; }

		/// <summary>
		/// The book title.
		/// </summary>
		[Column("title")]
		public string Title { get; set; }

		/// <summary>
		/// The book category.
		/// </summary>
		[Column("category")]
		public string Category { get; set; }

		/// <summary>
		/// The creation date.
		/// </summary>
		[Column("created")]
		public DateTime Created { get; set; }

		/// <summary>
		/// The cover filename (stored in gridfs).
		/// </summary>
		[Column("filename")]
		public string Filename { get; set; }

		/// <summary>
		/// The stories in the book.
		/// </summary>
		public ICollection<Story> Stories { get; set; }

		/// <summary>
		/// The tags for the book.
		/// </summary>
		public ICollection<Tag> Tags { get; set; }
	}
}