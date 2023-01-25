using AutoMapper;
using LtbDb.Core.Interfaces;
using LtbDb.Options;
using LtbDb.WebAPI.V1.Contracts.Responses;
using LtbDb.WebAPI.V1.Filter;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Mime;
using System.Threading.Tasks;

namespace LtbDb.WebAPI.V1.Controllers
{
	[ApiController]
	[ApiExplorerSettings(GroupName = "v1")]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	[Authorize(Policy = "AdministratorOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
	[ProducesResponseType(StatusCodes.Status401Unauthorized)]
	[ValidationFilter]
	public class SearchController : ControllerBase
	{
		private readonly IMapper Mapper;
		private readonly AppSettings AppSettings;
		private readonly ISearchService SearchService;

		public SearchController(IMapper mapper, IOptionsSnapshot<AppSettings> settings, ISearchService search)
		{
			Mapper = mapper;
			AppSettings = settings.Value;
			SearchService = search;
		}

		/// <summary>
		/// Search a book.
		/// </summary>
		/// <param name="query"></param>
		/// <returns></returns>
		[HttpGet]
		[ProducesResponseType(typeof(IList<SearchResponse>), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(IList<ErrorResponse>), StatusCodes.Status400BadRequest)]
		public async Task<IActionResult> Get([BindRequired] string query)
		{
			var _books = await SearchService.SearchAsync(query ?? string.Empty);

			return Ok(Mapper.Map<IList<SearchResponse>>(_books));
		}
	}
}