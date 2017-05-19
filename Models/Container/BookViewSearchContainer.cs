using System.Collections.Generic;

namespace ltbdb.Models
{
	public class BookViewSearchContainer
	{
		public IEnumerable<BookModel> Books { get; set; }
		public string Query { get; set; }
		public PageOffset PageOffset { get; set; }
	}
}