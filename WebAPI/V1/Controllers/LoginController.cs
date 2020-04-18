using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System;
using ltbdb.Extensions;
using ltbdb.Models;
using ltbdb.WebAPI.V1.Contracts.Responses;

namespace ltbdb.WebAPI.V1.Controllers
{
	[ApiController]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	public class LoginController : ControllerBase
	{
		public readonly Settings Options;
		public readonly JwtToken JwtToken;

		public LoginController(IOptionsSnapshot<Settings> settings, JwtToken token)
		{
			Options = settings.Value;
			JwtToken = token;
		}

		[HttpPost]
		public IActionResult Post([FromBody]LoginModel model)
		{
			if (ModelState.IsValid)
			{
				try
				{
					if (Options.Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase) && Options.Password.Equals(model.Password))
					{
						var _response = new AuthSuccessResponse
						{
							Token = JwtToken.CreateToken(Options.SecurityKey, model.Username, "Administrator", Options.TokenExpire),
							Type = "Bearer",
							ExpiresIn = Options.TokenExpire
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

			return BadRequest();
		}
	}
}