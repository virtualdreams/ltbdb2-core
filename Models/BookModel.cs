using System.Collections.Generic;
using System;

namespace LtbDb.Models
{
	public class BookModel
	{
		public int Id { get; set; }

		public int? Number { get; set; }

		public string Title { get; set; }

		public string Category { get; set; }

		public DateTime Created { get; set; }

		public DateTime Modified { get; set; }

		public string Filename { get; set; }

		public List<string> Stories { get; set; } = new List<string>();

		public List<string> Tags { get; set; } = new List<string>();
	}
}