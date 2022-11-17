using FluentValidation;
using LtbDb.FluentValidation;
using LtbDb.WebAPI.V1.Contracts.Requests;

namespace LtbDb.WebAPI.V1.Validators
{
	public class BookRequestValidator : AbstractValidator<BookRequest>
	{
		public BookRequestValidator()
		{
			RuleFor(r => r.Number)
				.NotEmpty()
				.WithMessage("Bitte gib eine Nummer ein.");

			RuleFor(r => r.Title)
				.NotEmpty()
				.WithMessage("Bitte gib einen Titel ein.")
				.MaximumLength(200)
				.WithMessage("Der Titel darf max. 200 Zeichen lang sein.");

			RuleFor(r => r.Category)
				.NotEmpty()
				.WithMessage("Bitte gib eine Reihe ein.")
				.MaximumLength(100)
				.WithMessage("Die Reihe darf max. 100 Zeichen lang sein.");

			RuleFor(r => r.Stories)
				//.MaximumLength(100)
				.MaximumLengthInArray(200)
				.WithMessage("Ein Eintrag darf max. 200 Zeichen lang sein.");

			RuleFor(r => r.Tags)
				//.MaximumLength(50)
				.MaximumLengthInArray(50)
				.WithMessage("Ein Eintrag darf max. 50 Zeichen lang sein.");
		}
	}
}