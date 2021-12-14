using AutoMapper;
using LtbDb.Core.Models;
using LtbDb.WebAPI.V1.Contracts.Requests;
using LtbDb.WebAPI.V1.Contracts.Responses;
using System.Linq;
using System;

namespace LtbDb.WebAPI.V1.MappingProfiles
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			#region domain -> response
			CreateMap<Book, BookResponse>()
				.ForMember(d => d.Stories, map => map.MapFrom(s => s.Stories
					.Select(x => x.Name)))
				.ForMember(d => d.Tags, map => map.MapFrom(s => s.Tags
					.Select(x => x.Name)))
				.ForSourceMember(s => s.Filename, map => map
					.DoNotValidate());

			CreateMap<Book, SearchResponse>()
				.ForMember(d => d.Id, map => map.MapFrom(s => s.Id))
				.ForMember(d => d.Title, map => map.MapFrom(s => s.Title));
			#endregion

			#region request -> domain
			CreateMap<BookRequest, Book>()
				.ForMember(d => d.Id, map => map
					.Ignore())
				.ForMember(d => d.Title, map => map.MapFrom(s => s.Title
					.Trim()))
				.ForMember(d => d.Category, map => map.MapFrom(s => s.Category
					.Trim()))
				.ForMember(d => d.Created, map => map
					.Ignore())
				.ForMember(d => d.Modified, map => map
					.Ignore())
				.ForMember(d => d.Filename, map => map
					.Ignore())
				.ForMember(d => d.Stories, map => map.MapFrom(s => s.Stories
					.Where(w => !String.IsNullOrEmpty(w))
					.Select(x => new Story { Name = x.Trim() })))
				.ForMember(d => d.Tags, map => map.MapFrom(s => s.Tags
					.Where(w => !String.IsNullOrEmpty(w))
					.Select(x => new Tag { Name = x.Trim() })));
			#endregion
		}
	}
}