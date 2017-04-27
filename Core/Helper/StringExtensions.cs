using System;
using System.Text;
using System.Text.RegularExpressions;
using MongoDB.Bson;

namespace ltbdb.Core.Helpers
{
    static public class StringExtensions
	{
		/// <summary>
		/// Get last n characters.
		/// </summary>
		/// <param name="value">The value.</param>
		/// <param name="len">Max length.</param>
		/// <returns>Last n characters.</returns>
		static public string GetLast(this string value, int len)
		{
			if(String.IsNullOrEmpty(value) || len >= value.Length)
				return value;

			return value.Substring(value.Length - len);
		}

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