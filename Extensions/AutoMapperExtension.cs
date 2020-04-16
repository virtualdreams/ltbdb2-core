using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using ltbdb.MappingProfiles;

namespace ltbdb.Extensions
{
	static public class AutoMapperExtensions
	{
		static public IServiceCollection AddAutoMapper(this IServiceCollection services)
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