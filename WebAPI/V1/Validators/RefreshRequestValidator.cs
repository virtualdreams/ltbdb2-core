using FluentValidation;
using ltbdb.WebAPI.V1.Contracts.Requests;

namespace ltbdb.WebAPI.V1.Validators
{
	public class RefreshRequestValidator : AbstractValidator<RefreshRequest>
	{
		public RefreshRequestValidator()
		{
			RuleFor(r => r.RefreshToken)
				.NotEmpty()
				.WithMessage("Das Refresh Token darf nicht leer sein.");
		}
	}
}