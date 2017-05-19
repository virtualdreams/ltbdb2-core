using System;
using System.Diagnostics;
using System.IO;

namespace ltbdb.Core.Helpers
{
	static public class GraphicsMagick
	{
		/// <summary>
		/// Path to GraphicsImage. If null, "gm" will used.
		/// </summary>
		static public string GraphicsImage { get; set; }

		/// <summary>
		/// Invoke "GraphicsMagick" via command line interface.
		/// </summary>
		/// <param name="source">The source image stream.</param>
		/// <param name="target">The target image stream.</param>
		/// <param name="arguments">GraphicsMagick arguments.</param>
		static public void PInvoke(Stream source, Stream target, string arguments)
		{
			using (var process = new Process())
			{
				process.StartInfo = new ProcessStartInfo
				{
					FileName = GraphicsImage ?? "gm",
					Arguments = arguments,
					RedirectStandardInput = true,
					RedirectStandardOutput = true,
					RedirectStandardError = true,
					UseShellExecute = false,
					CreateNoWindow = true
				};

				process.Start();

				source.CopyTo(process.StandardInput.BaseStream);
				process.StandardInput.Flush();
				process.StandardInput.Dispose();

				process.StandardOutput.BaseStream.CopyTo(target);

				var error = process.StandardError.ReadToEnd();
				process.WaitForExit();

				if (process.ExitCode != 0)
				{
					throw new Exception(error);
				}
			}
		}
	}
}
