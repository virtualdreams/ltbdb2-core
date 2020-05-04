using AutoMapper;
using System.Linq;
using System;
using ltbdb.Core.Models;
using ltbdb.WebAPI.V1.Contracts.Requests;
using ltbdb.WebAPI.V1.Contracts.Responses;

namespace ltbdb.WebAPI.V1.MappingProfiles
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			#region request -> domain
			CreateMap<BookRequest, Book>()
				.ForMember(d => d.Id, map => map.Ignore())
				.ForMember(d => d.Created, map => map.Ignore())
				.ForMember(d => d.Filename, map => map.Ignore())
				.ForMember(d => d.Stories, map => map.MapFrom(s => s.Stories.Select(x => new Story { Name = x })))
				.ForMember(d => d.Tags, map => map.MapFrom(s => s.Tags.Select(x => new Tag { Name = x })));
			#endregion

			#region domain -> response
			CreateMap<Book, BookResponse>()
				.ForMember(d => d.Stories, map => map.MapFrom(s => s.Stories.Select(x => x.Name)))
				.ForMember(d => d.Tags, map => map.MapFrom(s => s.Tags.Select(x => x.Name)))
				.ForSourceMember(s => s.Filename, map => map.DoNotValidate());
			#endregion
		}
	}
}