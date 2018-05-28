using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using ltbdb.Core.Helpers;
using ltbdb.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace ltbdb.WebAPI.Controllers
{
	[Produces("application/json")]
	[Route("api/v1/[controller]")]
	public class TokenController : Controller
	{
		public readonly Settings Options;

		public TokenController(Settings settings)
		{
			Options = settings;
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
						return Ok(new { Token = JwtTokenGenerator.Generate(Options.SecurityKey, model.Username, "Administrator") });
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