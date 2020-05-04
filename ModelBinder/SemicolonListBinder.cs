using Microsoft.AspNetCore.Mvc.ModelBinding;
using System.Linq;
using System.Threading.Tasks;
using System;

namespace ltbdb.ModelBinder
{
	public class SemicolonListBinder : IModelBinder
	{
		public Task BindModelAsync(ModelBindingContext bindingContext)
		{
			if (bindingContext == null)
			{
				throw new ArgumentNullException(nameof(bindingContext));
			}

			var modelName = bindingContext.ModelName;

			var valueProviderResult = bindingContext.ValueProvider.GetValue(modelName);
			if (valueProviderResult == ValueProviderResult.None)
			{
				return Task.CompletedTask;
			}

			bindingContext.ModelState.SetModelValue(modelName, valueProviderResult);

			var value = valueProviderResult.FirstValue;

			if (String.IsNullOrEmpty(value))
			{
				return Task.CompletedTask;
			}

			var model = value
				.Split(new[] { ';' })
				.ToList();

			bindingContext.Result = ModelBindingResult.Success(model);

			return Task.CompletedTask;
		}
	}
}