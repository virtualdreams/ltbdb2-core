using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System;
using ltbdb.Options;
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
		private readonly AppSettings AppSettings;
		private readonly BearerTokenService Token;

		public UserController(IOptionsSnapshot<AppSettings> settings, BearerTokenService token)
		{
			AppSettings = settings.Value;
			Token = token;
		}

		[HttpPost("authenticate")]
		public IActionResult Post([FromBody] AuthRequest model)
		{
			try
			{
				if (AppSettings.Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase) && AppSettings.Password.Equals(model.Password))
				{
					var _response = new AuthSuccessResponse
					{
						Token = Token.CreateToken(AppSettings.AccessTokenKey, model.Username, "Administrator", AppSettings.AccessTokenExpire),
						Type = "Bearer",
						ExpiresIn = AppSettings.AccessTokenExpire
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