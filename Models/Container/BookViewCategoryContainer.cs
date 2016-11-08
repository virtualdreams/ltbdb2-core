using System.Collections.Generic;

namespace ltbdb.Models
{
    public class BookViewCategoryContainer
	{
		public IEnumerable<BookModel> Books { get; set; }
		public string Category { get; set; }
		public PageOffset PageOffset { get; set; }
	}
}