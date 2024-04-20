using LtbDb.Core.Interfaces;
using LtbDb.Options;
using LtbDb.Services;
using LtbDb.WebAPI.V1.Contracts.Requests;
using LtbDb.WebAPI.V1.Contracts.Responses;
using LtbDb.WebAPI.V1.Filter;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Collections.Generic;
using System.Net.Mime;
using System;

namespace LtbDb.WebAPI.V1.Controllers
{
	[ApiController]
	[ApiExplorerSettings(GroupName = "v1")]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	[ValidationFilter]
	public class LoginController : ControllerBase
	{
		private readonly ILogger<LoginController> Log;

		private readonly AppSettings AppSettings;

		private readonly IUserService UserService;

		private readonly BearerTokenService TokenService;

		public LoginController(
			ILogger<LoginController> log,
			IOptionsSnapshot<AppSettings> settings,
			IUserService user,
			BearerTokenService token)
		{
			Log = log;
			AppSettings = settings.Value;
			UserService = user;
			TokenService = token;
		}

		/// <summary>
		/// Create an authentication token.
		/// </summary>
		/// <param name="model"></param>
		/// <returns></returns>
		[HttpPost]
		[ProducesResponseType(typeof(AuthSuccessResponse), StatusCodes.Status200OK)]
		[ProducesResponseType(typeof(IList<ErrorResponse>), StatusCodes.Status400BadRequest)]
		[ProducesResponseType(StatusCodes.Status401Unauthorized)]
		public IActionResult Post([FromBody] AuthRequest model)
		{
			try
			{
				if (UserService.Login(model.Username, model.Password))
				{
					var _response = new AuthSuccessResponse
					{
						Token = TokenService.CreateToken(AppSettings.JwtSigningKey, model.Username, "Administrator", AppSettings.JwtExpireTime),
						Type = "Bearer",
						ExpiresIn = AppSettings.JwtExpireTime
					};

					Log.LogInformation("Login successful.");

					return Ok(_response);
				}

				Log.LogInformation("Login failed. Unauthorized.");

				return Unauthorized();
			}
			catch (Exception e)
			{
				Log.LogError(e, "Login failed.");

				return StatusCode(500);
			}
		}
	}
}