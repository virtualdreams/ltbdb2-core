using System;
using System.Collections.Generic;
using FluentValidation;

namespace ltbdb.Validators
{
	public static class CustomValidators
	{
		public static IRuleBuilderOptions<T, IList<string>> MaximumLengthInArray<T>(this IRuleBuilder<T, IList<string>> ruleBuilder, int length)
		{
			return ruleBuilder.Must(list =>
			{
				foreach (var item in list)
				{
					if (item == null)
						continue;

					if (item.Length > length)
						return false;
				}
				return true;
			});
		}

		public static IRuleBuilderOptions<T, string> MaximumLengthInArrayString<T>(this IRuleBuilder<T, string> ruleBuilder, int length, char separator)
		{
			return ruleBuilder.Must(str =>
			{
				if (str != null)
				{
					var list = str.Split(new[] { separator }, StringSplitOptions.RemoveEmptyEntries);
					foreach (var item in list)
					{
						if (item.Length > length)
						{
							return false;
						}
					}
				}
				return true;
			});
		}

		public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
		{
			return ruleBuilder.Must(list => list.Count < num);
		}
	}
}