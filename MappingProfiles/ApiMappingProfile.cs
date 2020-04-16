using AutoMapper;
using System.Linq;
using System;
using ltbdb.Core.Models;
using ltbdb.WebAPI.Contracts.V1.Requests;

namespace ltbdb.MappingProfiles
{
	public class ApiMappingProfile : Profile
	{
		public ApiMappingProfile()
		{
			CreateMap<BookApiRequest, Book>()
				.ForMember(d => d.Id, map => map.Ignore())
				.ForMember(d => d.Created, map => map.Ignore())
				.ForMember(d => d.Filename, map => map.Ignore())
				.ForMember(d => d.Stories, map => map.MapFrom(s => s.Stories.Where(w => !String.IsNullOrEmpty(w)).Select(x => new Story { Name = x.Trim() })))
				.ForMember(d => d.Tags, map => map.MapFrom(s => s.Tags.Where(w => !String.IsNullOrEmpty(w)).Select(x => x.Trim()).Distinct().Select(x => new Tag { Name = x })));
		}
	}
}