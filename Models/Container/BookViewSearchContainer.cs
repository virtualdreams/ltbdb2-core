using System.Collections.Generic;

namespace LtbDb.Models
{
	public class BookViewSearchContainer
	{
		public IEnumerable<BookModel> Books { get; set; }
		public string Query { get; set; }
		public PageOffset PageOffset { get; set; }
	}
}