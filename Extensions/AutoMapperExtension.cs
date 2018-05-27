using AutoMapper;
using Microsoft.Extensions.DependencyInjection;
using System.Linq;
using System;
using ltbdb.Core.Models;
using ltbdb.Models;

namespace ltbdb.Extensions
{
	static public class AutoMapperExtensions
	{
		static public IServiceCollection AddAutoMapper(this IServiceCollection services)
		{
			var _autoMapperConfig = new MapperConfiguration(config =>
			{
				config.AllowNullCollections = false;

				config.CreateMap<Book, BookModel>();
				config.CreateMap<Book, BookWriteModel>()
					.ForMember(d => d.Tags, map => map.MapFrom(s => String.Join("; ", s.Tags)))
					.ForMember(d => d.Image, map => map.Ignore())
					.ForMember(d => d.Remove, map => map.Ignore());

				config.CreateMap<BookWriteModel, Book>()
					.ForMember(d => d.Title, map => map.MapFrom(s => s.Title.Trim()))
					.ForMember(d => d.Category, map => map.MapFrom(s => s.Category.Trim()))
					.ForMember(s => s.Created, map => map.Ignore())
					.ForMember(d => d.Filename, map => map.Ignore())
					.ForMember(d => d.Stories, map => map.MapFrom(s => s.Stories.Where(w => !String.IsNullOrEmpty(w)).Select(x => x.Trim())))
					.ForMember(d => d.Tags, map => map.MapFrom(s => s.Tags.Split(new char[] { ';' }, StringSplitOptions.RemoveEmptyEntries).Select(x => x.Trim()).Where(w => !String.IsNullOrEmpty(w)).Distinct()))
					.ForSourceMember(s => s.Image, map => map.Ignore())
					.ForSourceMember(s => s.Remove, map => map.Ignore());

				config.CreateMap<BookWriteApiModel, Book>()
					.ForMember(d => d.Id, map => map.Ignore())
					.ForMember(d => d.Created, map => map.Ignore())
					.ForMember(d => d.Filename, map => map.Ignore());
			});

			_autoMapperConfig.AssertConfigurationIsValid();

			services.AddSingleton<IMapper>(am => _autoMapperConfig.CreateMapper());

			return services;
		}
	}
}