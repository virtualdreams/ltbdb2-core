using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Distributed;
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
		private readonly IDistributedCache Cache;
		private readonly TokenService Token;

		public UserController(IOptionsSnapshot<Settings> settings, IDistributedCache cache, TokenService token)
		{
			Options = settings.Value;
			Cache = cache;
			Token = token;
		}

		[HttpPost("authenticate")]
		public IActionResult Post([FromBody] AuthRequest model)
		{
			try
			{
				if (Options.Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase) && Options.Password.Equals(model.Password))
				{
					var _refreshId = Guid.NewGuid().ToString();

					var _response = new AuthSuccessResponse
					{
						AccessToken = Token.CreateAccessToken(Options.AccessTokenKey, model.Username, "Administrator", Options.AccessTokenExpire),
						RefreshToken = Token.CreateRefreshToken(Options.AccessTokenKey, model.Username, _refreshId),
						Type = "Bearer",
						ExpiresIn = Options.AccessTokenExpire
					};

					Cache.SetString(model.Username, _refreshId, new DistributedCacheEntryOptions
					{
						AbsoluteExpiration = DateTimeOffset.UtcNow.AddDays(7)
					});

					return Ok(_response);
				}

				return Unauthorized();
			}
			catch (Exception)
			{
				return StatusCode(500);
			}
		}

		[HttpPost("refresh-token")]
		public IActionResult RefreshToken([FromBody] RefreshRequest model)
		{
			try
			{
				var _principal = Token.GetPrincipalFromRefreshToken(model.RefreshToken, Options.AccessTokenKey);
				var _principalRefreshName = _principal.Identity.Name;
				var _principalTokenId = _principal.FindFirst(TokenService.TokenId)?.Value;
				var _oldTokenId = Cache.GetString(_principal.Identity.Name);

				if (!_oldTokenId.Equals(_principalTokenId) || !Options.Username.Equals(_principalRefreshName))
				{
					return Unauthorized();
				}

				var _response = new RefreshSuccessResponse
				{
					AccessToken = Token.CreateAccessToken(Options.AccessTokenKey, _principal.Identity.Name, "Administrator", Options.AccessTokenExpire),
					Type = "Bearer",
					ExpiresIn = Options.AccessTokenExpire
				};

				return Ok(_response);
			}
			catch (Exception)
			{
				return Unauthorized();
			}
		}

		[HttpPost("revoke-token")]
		[Authorize(Policy = "AdministratorOnly", AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
		public IActionResult RevokeToken()
		{
			Cache.Remove(User.Identity.Name);

			return NoContent();
		}
	}
}