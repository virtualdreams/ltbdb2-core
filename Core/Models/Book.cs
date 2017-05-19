using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System;

namespace ltbdb.Core.Models
{
	/// <summary>
	/// Object to store a book.
	/// </summary>
	[BsonIgnoreExtraElements]
	public class Book
	{
		/// <summary>
		/// The book id.
		/// </summary>
		[BsonId]
		[BsonIgnoreIfDefault]
		public ObjectId Id { get; set; }

		/// <summary>
		/// The book number.
		/// </summary>
		public int Number { get; set; }

		/// <summary>
		/// The book title.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// The book category.
		/// </summary>
		public string Category { get; set; }

		/// <summary>
		/// The creation date.
		/// </summary>
		[BsonDateTimeOptions(Kind = DateTimeKind.Local)]
		public DateTime Created { get; set; }

		/// <summary>
		/// The cover filename (stored in gridfs).
		/// </summary>
		public string Filename { get; set; }

		private string[] _stories = new string[] { };

		/// <summary>
		/// The stories in the book.
		/// </summary>
		public string[] Stories
		{
			get
			{
				return _stories;
			}
			set
			{
				if (value != null)
				{
					_stories = value;
				}
			}
		}

		private string[] _tags = new string[] { };

		/// <summary>
		/// The tags for the book.
		/// </summary>
		public string[] Tags
		{
			get
			{
				return _tags;
			}
			set
			{
				if (value != null)
				{
					_tags = value;
				}
			}
		}
	}
}