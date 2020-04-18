using FluentValidation;
using ltbdb.WebAPI.V1.Contracts.Requests;

namespace ltbdb.WebAPI.V1.Validators
{
	public class LoginRequestValidator : AbstractValidator<LoginRequest>
	{
		public LoginRequestValidator()
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