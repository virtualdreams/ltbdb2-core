using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System.Net.Mime;
using System;
using ltbdb.Extensions;
using ltbdb.Models;

namespace ltbdb.WebAPI.Controllers.V1
{
	[ApiController]
	[Produces(MediaTypeNames.Application.Json)]
	[Route("api/v1/[controller]")]
	public class LoginController : ControllerBase
	{
		public readonly Settings Options;
		public readonly JwtTokenGenerator JwtToken;

		public LoginController(IOptionsSnapshot<Settings> settings, JwtTokenGenerator token)
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
						return Ok(new
						{
							Token = JwtToken.GenerateToken(Options.SecurityKey, model.Username, "Administrator", Options.TokenExpire),
							Type = "Bearer",
							ExpiresIn = Options.TokenExpire
						});
					}

					return StatusCode(403);
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