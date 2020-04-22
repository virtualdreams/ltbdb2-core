using FluentValidation;
using Microsoft.Extensions.Options;
using ltbdb.WebAPI.V1.Contracts.Requests;

namespace ltbdb.WebAPI.V1.Validators
{
	public class ImageRequestValidator : AbstractValidator<ImageRequest>
	{
		public ImageRequestValidator(IOptionsSnapshot<Settings> settings)
		{
			RuleFor(r => r.Image)
				.Cascade(CascadeMode.StopOnFirstFailure)
				.NotNull()
				.WithMessage("Bild ist erforderlich.")
				.Must(m => m.Length > 0)
				.WithMessage("Bild darf nicht die Größe 0 haben.");
		}
	}
}