using System;

namespace LtbDb.Core.Internal
{
	public static class StringExtensions
	{
		/// <summary>
		/// Combines two pathes.
		/// </summary>
		/// <param name="baseUri">The base path.</param>
		/// <param name="relativeUri">The additional path.</param>
		/// <returns>The combined path.</returns>
		public static string Combine(this string baseUri, string relativeUri)
		{
			if (baseUri == null) throw new ArgumentNullException(nameof(baseUri));
			if (relativeUri == null) throw new ArgumentNullException(nameof(relativeUri));

			if (!baseUri.EndsWith("/"))
				baseUri += "/";

			return String.Format("{0}{1}", baseUri, relativeUri);
		}
	}
}