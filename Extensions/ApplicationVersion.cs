using System.Reflection;

namespace ltbdb.Extensions
{
	/// <summary>
	/// Get assembly version informations.
	/// </summary>
	static public class ApplicationVersion
	{
		/// <summary
		/// Get file version.
		/// </summary>
		/// <returns>File version.</returns>
		static public string FileVersion() => $"{System.Reflection.Assembly.GetEntryAssembly().GetName().Version}";

		/// <summary>
		/// Get informational version.
		/// </summary>
		/// <returns>Informational version.</returns>
		static public string InfoVersion() => $"{System.Reflection.Assembly.GetEntryAssembly().GetCustomAttribute<AssemblyInformationalVersionAttribute>().InformationalVersion}";
	}
}