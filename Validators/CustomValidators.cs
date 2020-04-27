using FluentValidation;
using System.Collections.Generic;
using System;

namespace ltbdb.Validators
{
	public static class CustomValidators
	{
		public static IRuleBuilderOptions<T, IList<string>> MaximumLengthInArray<T>(this IRuleBuilder<T, IList<string>> ruleBuilder, int length)
		{
			return ruleBuilder.Must((objectRoot, list, context) =>
			{
				context.MessageFormatter.AppendArgument("MaxLength", length);

				foreach (var item in list)
				{
					if (item == null)
						continue;

					if (item.Length > length)
						return false;
				}
				return true;
			})
			.WithMessage("The length of an item of '{PropertyName}' must be {MaxLength} characters or fewer.");
		}

		public static IRuleBuilderOptions<T, string> MaximumLengthInArrayString<T>(this IRuleBuilder<T, string> ruleBuilder, int length, char separator)
		{
			return ruleBuilder.Must((objectRoot, str, context) =>
			{
				context.MessageFormatter.AppendArgument("MaxLength", length);

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
			})
			.WithMessage("The length of an item of '{PropertyName}' must be {MaxLength} characters or fewer.");
		}

		public static IRuleBuilderOptions<T, IList<TElement>> ListMustContainFewerThan<T, TElement>(this IRuleBuilder<T, IList<TElement>> ruleBuilder, int num)
		{
			return ruleBuilder.Must((objectRoot, list, context) =>
			{
				context.MessageFormatter.AppendArgument("MaxElements", num);

				return list.Count < num;
			})
			.WithMessage("'{PropertyName}' must contain fewer than {MaxElements} items.");
		}
	}
}