using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace ltbdb.Core.Models
{
	[Table("book")]
	public class Book
	{
		[Column("id")]
		public int Id { get; set; }

		[Column("number")]
		public int Number { get; set; }

		[Column("title")]
		public string Title { get; set; }

		[Column("category")]
		public string Category { get; set; }

		[Column("created")]
		public DateTime Created { get; set; }

		[Column("filename")]
		public string Filename { get; set; }

		public List<Story> Stories { get; set; } = new List<Story>();

		public List<Tag> Tags { get; set; } = new List<Tag>();
	}
}