using System.Collections.Generic;
using System;

namespace LtbDb.WebAPI.V1.Contracts.Responses
{
	public class BookResponse
	{
		public int Id { get; set; }

		public int? Number { get; set; }

		public string Title { get; set; }

		public string Category { get; set; }

		public DateTime Created { get; set; }

		public DateTime Modified { get; set; }

		public List<string> Stories { get; set; } = new List<string>();

		public List<string> Tags { get; set; } = new List<string>();
	}
}