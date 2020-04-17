using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ltbdb.MappingProfiles;

namespace ltbdb.Extensions
{
	public static class AutoMapperExtensions
	{
		public static IServiceCollection AddAutoMapper(this IServiceCollection services)
		{
			var _autoMapperConfig = new MapperConfiguration(config =>
			{
				config.AllowNullCollections = false;

				config.AddProfile<WebMappingProfile>();
				config.AddProfile<ApiMappingProfile>();
			});

			_autoMapperConfig.AssertConfigurationIsValid();

			services.AddSingleton<IMapper>(am => _autoMapperConfig.CreateMapper());

			return services;
		}
	}
}