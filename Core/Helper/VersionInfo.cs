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
				return Microsoft.Extensions.PlatformAbstractions.PlatformServices.Default.Application.ApplicationVersion;
			}
		}
	}
}
