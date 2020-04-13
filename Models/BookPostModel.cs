using Microsoft.AspNetCore.Http;
using System.Collections.Generic;

namespace ltbdb.Models
{
	public class BookPostModel
	{
		public int Id { get; set; }

		public int? Number { get; set; }

		public string Title { get; set; }

		public string Category { get; set; }

		public string Filename { get; set; }

		public List<string> Stories { get; set; } = new List<string>();

		public string Tags { get; set; }

		public IFormFile Image { get; set; }

		public bool Remove { get; set; }
	}
}