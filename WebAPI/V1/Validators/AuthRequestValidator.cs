using FluentValidation;
using LtbDb.WebAPI.V1.Contracts.Requests;

namespace LtbDb.WebAPI.V1.Validators
{
	public class AuthRequestValidator : AbstractValidator<AuthRequest>
	{
		public AuthRequestValidator()
		{
			RuleFor(r => r.Username)
				.NotEmpty()
				.WithMessage("Bitte gib einen Benutzernamen ein.");

			RuleFor(r => r.Password)
				.NotEmpty()
				.WithMessage("Bitte gib ein Passwort ein.");
		}
	}
}