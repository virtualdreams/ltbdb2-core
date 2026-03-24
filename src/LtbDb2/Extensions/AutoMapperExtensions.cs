using AutoMapper.Internal;
using AutoMapper;
using Microsoft.Extensions.DependencyInjection;

namespace LtbDb.Extensions
{
	public static class AutoMapperExtensions
	{
		public static IServiceCollection AddAutoMapper(this IServiceCollection services)
		{
			var mappingConfiguration = new MapperConfiguration(config =>
			{
				config.Internal().ForAllMaps((_, mapping) => mapping.MaxDepth(64));
				config.AllowNullCollections = false;

				config.AddMaps(typeof(Startup));
				// config.AddProfile<WebMappingProfile>();
				// config.AddProfile<ApiMappingProfile>();
			});

			mappingConfiguration.AssertConfigurationIsValid();

			services.AddSingleton(mappingConfiguration.CreateMapper());

			return services;
		}
	}
}