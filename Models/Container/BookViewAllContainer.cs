using System.Collections.Generic;

namespace LtbDb.Models
{
	public class BookViewAllContainer
	{
		public IEnumerable<BookModel> Books { get; set; }
		public PageOffset PageOffset { get; set; }
	}
}