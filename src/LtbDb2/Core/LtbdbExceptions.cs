using System;

namespace LtbDb.Core
{
	public class LtbdbException : Exception
	{
		public LtbdbException(string message)
			: base(message)
		{ }
	}

	public class LtbdbInvalidFilenameException : LtbdbException
	{
		public LtbdbInvalidFilenameException()
			: base("Invalid filename.")
		{ }
	}

	public class LtbdbRenameCategoryException : LtbdbException
	{
		public LtbdbRenameCategoryException()
			: base("Rename category names must be non-zero.")
		{ }
	}

	public class LtbdbNotFoundException : LtbdbException
	{
		public LtbdbNotFoundException()
			: base("Book not found.")
		{ }
	}

	public class LtbdbDuplicateEntryException : LtbdbException
	{
		public LtbdbDuplicateEntryException()
			: base("Duplicate book entry not allowed.")
		{ }
	}
}