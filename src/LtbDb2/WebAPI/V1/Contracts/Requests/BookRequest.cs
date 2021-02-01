using System.Collections.Generic;

namespace LtbDb.WebAPI.V1.Contracts.Requests
{
	public class BookRequest
	{
		public int Number { get; set; }

		public string Title { get; set; }

		public string Category { get; set; }

		public List<string> Stories { get; set; } = new List<string>();

		public List<string> Tags { get; set; } = new List<string>();
	}
}