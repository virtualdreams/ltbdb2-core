using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using ltbdb.ModelBinder;

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

		[BindProperty(BinderType = typeof(SemicolonListBinder))]
		public List<string> Tags { get; set; } = new List<string>();

		public IFormFile Image { get; set; }

		public bool Remove { get; set; }
	}
}