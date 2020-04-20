using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using ltbdb.WebAPI.V1.Contracts.Responses;

namespace ltbdb.WebAPI.V1.Filter
{
	[AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
	public class ValidationFilterAttribute : Attribute, IAsyncActionFilter
	{
		public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
		{
			if (!context.ModelState.IsValid)
			{
				var _errorsInModelState = context.ModelState
					.Where(w => w.Value.Errors.Count > 0)
					.ToDictionary(d => d.Key, d => d.Value.Errors.Select(s => s.ErrorMessage)).ToArray();

				var _response = new List<ErrorResponse>();

				foreach (var error in _errorsInModelState)
				{
					_response.Add(new ErrorResponse
					{
						Field = error.Key,
						Messages = error.Value.ToList()
					});
				}

				context.Result = new BadRequestObjectResult(_response);
				return;
			}

			await next();
		}
	}
}