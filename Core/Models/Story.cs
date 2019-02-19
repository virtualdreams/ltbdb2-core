using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace ltbdb.Core.Models
{
	[Table("story")]
	public class Story
	{
		[Column("id")]
		public int Id { get; set; }

		[Column("name")]
		public string Name { get; set; }

		[Column("bookid")]
		public int BookId { get; set; }

		public Book Book { get; set; }
	}
}