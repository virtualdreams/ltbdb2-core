using System;

namespace ltbdb
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
}