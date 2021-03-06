﻿using System.Collections.Generic;

namespace LtbDb.Models
{
	public class BookViewTagContainer
	{
		public IEnumerable<BookModel> Books { get; set; }
		public string Tag { get; set; }
		public PageOffset PageOffset { get; set; }
	}
}