using FluentValidation;
using System.Collections.Generic;
using System;
using ltbdb.Models;

namespace ltbdb.Validators
{
	public class BookPostModelValidator : AbstractValidator<BookPostModel>
	{
		public BookPostModelValidator()
		{
			RuleFor(r => r.Number)
				.NotEmpty()
				.WithMessage("Bitte gib eine Nummer ein.");

			RuleFor(r => r.Title)
				.NotEmpty()
				.WithMessage("Bitte gib einen Titel ein.")
				.MaximumLength(100)
				.WithMessage("Der Titel darf max. 100 Zeichen lang sein.");

			RuleFor(r => r.Category)
				.NotEmpty()
				.WithMessage("Bitte gib eine Kategorie ein.")
				.MaximumLength(100)
				.WithMessage("Die Kategorie darf max. 100 Zeichen lang sein.");

			RuleFor(r => r.Stories)
				//.MaximumLength(100)
				.Must(MaximumLengthInArray)
				.WithMessage("Ein Eintrag darf max. 100 Zeichen lang sein.");

			RuleFor(r => r.Tags)
				.Must((model, value) =>
				{
					var _tmp = (value as String);
					if (_tmp != null)
					{
						var _list = _tmp.Split(new[] { ';' }, StringSplitOptions.RemoveEmptyEntries);
						foreach (var _item in _list)
						{
							if (_item.Length > 50)
							{
								return false;
							}
						}
					}
					return true;
				})
				.WithMessage("Ein Tag darf max. 50 Zeichen lang sein.");
		}

		private bool MaximumLengthInArray(List<string> value)
		{
			foreach (var item in value)
			{
				if (item == null)
					continue;

				if (item.Length > 100)
					return false;
			}
			return true;
		}
	}
}