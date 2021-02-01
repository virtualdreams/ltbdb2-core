using FluentValidation;
using LtbDb.FluentValidation;
using LtbDb.Models;

namespace LtbDb.Validators
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
				.MaximumLength(200)
				.WithMessage("Der Titel darf max. 200 Zeichen lang sein.");

			RuleFor(r => r.Category)
				.NotEmpty()
				.WithMessage("Bitte gib eine Kategorie ein.")
				.MaximumLength(100)
				.WithMessage("Die Kategorie darf max. 100 Zeichen lang sein.");

			RuleFor(r => r.Stories)
				//.MaximumLength(100)
				.MaximumLengthInArray(200)
				.WithMessage("Ein Eintrag darf max. 200 Zeichen lang sein.");

			RuleFor(r => r.Tags)
				.MaximumLengthInArray(50)
				.WithMessage("Ein Tag darf max. 50 Zeichen lang sein.");

			When(w => w.Image != null, () =>
			{
				RuleFor(r => r.Image)
					.Must(m => m.Length > 0)
					.WithMessage("Bild darf nicht die Größe 0 haben.");
			});
		}
	}
}