using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ltbdb.MappingProfiles;

namespace ltbdb.Extensions
{
	public static class AutoMapperExtensions
	{
		public static IServiceCollection AddAutoMapper(this IServiceCollection services)
		{
			var mappingConfiguration = new MapperConfiguration(config =>
			{
				config.AllowNullCollections = false;

				config.AddProfile<WebMappingProfile>();
				config.AddProfile<ApiMappingProfile>();
			});

			mappingConfiguration.AssertConfigurationIsValid();

			services.AddSingleton(mappingConfiguration.CreateMapper());

			return services;
		}
	}
}