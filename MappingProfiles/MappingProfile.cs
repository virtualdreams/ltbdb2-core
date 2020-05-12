using AutoMapper;
using System.Linq;
using System;
using ltbdb.Core.Models;
using ltbdb.Models;

namespace ltbdb.MappingProfiles
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			#region domain -> view
			CreateMap<Book, BookModel>()
				.ForMember(d => d.Stories, map => map.MapFrom(s => s.Stories
					.Select(x => x.Name)))
				.ForMember(d => d.Tags, map => map.MapFrom(s => s.Tags
					.Select(x => x.Name)));

			CreateMap<Book, BookPostModel>()
				.ForMember(d => d.Stories, map => map.MapFrom(s => s.Stories
					.Select(x => x.Name)))
				.ForMember(d => d.Tags, map => map.MapFrom(s => s.Tags
					.Select(x => x.Name)))
				.ForMember(d => d.Image, map => map
					.Ignore())
				.ForMember(d => d.Remove, map => map
					.Ignore());
			#endregion

			#region web -> domain
			CreateMap<BookPostModel, Book>()
				.ForMember(d => d.Title, map => map.MapFrom(s => s.Title
					.Trim()))
				.ForMember(d => d.Category, map => map.MapFrom(s => s.Category
					.Trim()))
				.ForMember(s => s.Created, map => map
					.Ignore())
				.ForMember(d => d.Filename, map => map
					.Ignore())
				.ForMember(d => d.Stories, map => map.MapFrom(s => s.Stories
					.Where(w => !String.IsNullOrEmpty(w))
					.Select(x => new Story { Name = x.Trim() })))
				.ForMember(d => d.Tags, map => map.MapFrom(s => s.Tags
					.Where(w => !String.IsNullOrEmpty(w))
					.Select(x => new Tag { Name = x.Trim() })))
				.ForSourceMember(s => s.Image, map => map
					.DoNotValidate())
				.ForSourceMember(s => s.Remove, map => map
					.DoNotValidate());
			#endregion
		}
	}
}