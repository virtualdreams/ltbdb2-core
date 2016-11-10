using System.Diagnostics;
using System.Reflection;

namespace ltbdb.Core.Helpers
{
    static public class VersionInfo
	{
		/// <summary>
		/// Get the file version.
		/// </summary>
		public static string FileVersion
		{
			get
			{
				var asm = Assembly.GetEntryAssembly();
				var fvi = FileVersionInfo.GetVersionInfo(asm.Location);

				return fvi.FileVersion;
			}
		}

		/// <summary>
		/// Get the product version.
		/// </summary>
		public static string ProductVersion
		{
			get
			{
				var asm = Assembly.GetEntryAssembly();
				var fvi = FileVersionInfo.GetVersionInfo(asm.Location);

				return fvi.ProductVersion;
			}
		}
	}
}
