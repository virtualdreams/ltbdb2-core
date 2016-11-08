using System.Collections.Generic;

namespace ltbdb.Models
{
    public class BookViewAllContainer
	{
		public IEnumerable<BookModel> Books { get; set; }
		public PageOffset PageOffset { get; set; }
	}
}