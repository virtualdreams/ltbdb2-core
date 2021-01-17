using System.Collections.Generic;

namespace LtbDb.Models
{
	public class BookViewContainer
	{
		public IEnumerable<BookModel> Books { get; set; }
	}
}