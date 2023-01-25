using LtbDb.ModelBinder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LtbDb.Models
{
	public class BookPostModel
	{
		public int Id { get; set; }

		public int? Number { get; set; }

		public string Title { get; set; }

		public string Category { get; set; }

		public string Filename { get; set; }

		public IList<string> Stories { get; set; } = new List<string>();

		[BindProperty(BinderType = typeof(SemicolonListBinder))]
		public IList<string> Tags { get; set; } = new List<string>();

		public IFormFile Image { get; set; }

		public bool Remove { get; set; }
	}
}