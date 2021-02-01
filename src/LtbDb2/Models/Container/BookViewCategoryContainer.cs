using System.Collections.Generic;

namespace LtbDb.Models
{
	public class BookViewCategoryContainer
	{
		public IEnumerable<BookModel> Books { get; set; }
		public string Category { get; set; }
		public PageOffset PageOffset { get; set; }
	}
}