using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System;

namespace ltbdb.Core.Models
{
	[Table("book")]
	public class Book
	{
		[Column("id")]
		[Required]
		public int Id { get; set; }

		[Column("number")]
		[Required]
		public int Number { get; set; }

		[Column("title")]
		[Required]
		[MaxLength(100)]
		public string Title { get; set; }

		[Column("category")]
		[Required]
		[MaxLength(100)]
		public string Category { get; set; }

		[Column("created")]
		[Required]
		public DateTime Created { get; set; }

		[Column("modified")]
		[Required]
		public DateTime Modified { get; set; }

		[Column("filename")]
		[MaxLength(100)]
		public string Filename { get; set; }

		public List<Story> Stories { get; set; } = new List<Story>();

		public List<Tag> Tags { get; set; } = new List<Tag>();
	}
}