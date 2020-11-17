using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System;
using ltbdb.Services;
using ltbdb.WebAPI.V1.Contracts.Requests;
using ltbdb.WebAPI.V1.Contracts.Responses;
using ltbdb.WebAPI.V1.Filter;

namespace ltbdb.WebAPI.V1.Controllers
{
	[ApiController]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	[ValidationFilter]
	public class UserController : ControllerBase
	{
		private readonly Settings Options;
		private readonly TokenService Token;

		public UserController(IOptionsSnapshot<Settings> settings, TokenService token)
		{
			Options = settings.Value;
			Token = token;
		}

		[HttpPost("authenticate")]
		public IActionResult Post([FromBody] AuthRequest model)
		{
			try
			{
				if (Options.Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase) && Options.Password.Equals(model.Password))
				{
					var _response = new AuthSuccessResponse
					{
						Token = Token.CreateToken(Options.AccessTokenKey, model.Username, "Administrator", Options.AccessTokenExpire),
						Type = "Bearer",
						ExpiresIn = Options.AccessTokenExpire
					};

					return Ok(_response);
				}

				return Unauthorized();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}
	}
}