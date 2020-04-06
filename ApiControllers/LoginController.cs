using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using ltbdb.Extensions;
using ltbdb.Models;

namespace ltbdb.WebAPI.Controllers
{
	[Produces("application/json")]
	[Route("api/v1/[controller]")]
	public class LoginController : Controller
	{
		public readonly Settings Options;

		public LoginController(IOptionsSnapshot<Settings> settings)
		{
			Options = settings.Value;
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
							Token = JwtTokenGenerator.Generate(Options.SecurityKey, model.Username, "Administrator", Options.TokenExpire),
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