using FluentValidation;
using LtbDb.WebAPI.V1.Contracts.Requests;

namespace LtbDb.WebAPI.V1.Validators
{
	public class ImageRequestValidator : AbstractValidator<ImageRequest>
	{
		public ImageRequestValidator()
		{
			RuleFor(r => r.Image)
				.Cascade(CascadeMode.Stop)
				.NotNull()
				.WithMessage("Bild ist erforderlich.")
				.Must(m => m.Length > 0)
				.WithMessage("Bild darf nicht die Größe 0 haben.");
		}
	}
}