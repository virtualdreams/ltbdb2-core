using System.Text.RegularExpressions;
using System;

namespace ltbdb.Extensions
{
	public static class LtbdbExtensions
	{
		/// <summary>
		/// Slugify the string.
		/// </summary>
		/// <param name="value">The string to slugify.</param>
		/// <param name="maxLength">Max length of text.</param>
		/// <returns>Slugified string.</returns>
		public static string ToSlug(this string value, int maxLength = 100)
		{
			if (String.IsNullOrEmpty(value))
				return String.Empty;

			// convert to lower case
			value = value.ToLowerInvariant();

			// remove all accents
			//var bytes = Encoding.GetEncoding("Cyrillic").GetBytes(value);
			//value = Encoding.ASCII.GetString(bytes);

			// replace spaces
			value = Regex.Replace(value, @"\s", "-");

			// replace underline
			value = Regex.Replace(value, @"_", "-");

			// replace german umlauts
			value = value.Replace("ä", "ae").Replace("ö", "oe").Replace("ü", "ue").Replace("ß", "ss");

			// remove invalid chars
			value = Regex.Replace(value, @"[^a-z0-9\s-]", "-");

			// trim dashes from end
			value = value.Trim('-');

			// replace double occurences of '-'
			value = Regex.Replace(value, @"([-]){2,}", "$1");

			// max length of text
			return value.Substring(0, value.Length <= maxLength ? value.Length : maxLength);
		}
	}
}