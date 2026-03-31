using FluentValidation.Results;
using LtbDb.WebAPI.V1.Contracts.Responses;
using System.Collections.Generic;

namespace LtbDb.WebAPI.V1.FluentValidation
{
	public static class FluentValidationExtensions
	{
		public static object ToBadRequest(this ValidationResult result)
		{
			if (!result.IsValid)
			{
				var _response = new List<ErrorResponse>();

				foreach (var error in result.Errors)
				{
					_response.Add(new ErrorResponse
					{
						Field = error.PropertyName,
						Messages = [error.ErrorMessage]
					});
				}

				return _response;
			}

			return null;
		}
	}
}