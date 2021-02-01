using System.Collections.Generic;
using System.Linq;
using System;

namespace LtbDb.Core.Internal
{
	public static class EnumerableExtensions
	{
		public static IEnumerable<T> Distinct<T, TKey>(this IEnumerable<T> items, Func<T, TKey> property)
		{
			return items.GroupBy(property).Select(x => x.First());
		}
	}
}