using FluentValidation;
using System.Collections.Generic;
using ltbdb.WebAPI.Contracts.V1.Requests;

namespace ltbdb.Validators
{
	public class BookApiRequestValidator : AbstractValidator<BookApiRequest>
	{
		public BookApiRequestValidator()
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
				.MaximumLengthInArray(100)
				.WithMessage("Ein Eintrag darf max. 100 Zeichen lang sein.");

			RuleFor(r => r.Tags)
				//.MaximumLength(50)
				.MaximumLengthInArray(50)
				.WithMessage("Ein Eintrag darf max. 50 Zeichen lang sein.");
		}
	}
}