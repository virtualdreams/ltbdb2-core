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

				// var _errorsInModelState = context.ModelState
				// 	.Where(w => w.Value.Errors.Count > 0)
				// 	.ToDictionary(d => d.Key, d => d.Value.Errors.Select(s => s.ErrorMessage)).ToArray();

				// var _response = new List<ErrorResponse>();

				// foreach (var error in _errorsInModelState)
				// {
				// 	_response.Add(new ErrorResponse
				// 	{
				// 		Field = error.Key,
				// 		Messages = error.Value.ToList()
				// 	});
				// }

				// context.Result = new BadRequestObjectResult(_response);
				// return true;
			}

			return null;
		}
	}
}