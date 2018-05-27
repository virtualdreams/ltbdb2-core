using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
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
			if(ModelState.IsValid)
			{
				try
				{
					if(Options.Username.Equals(model.Username, StringComparison.OrdinalIgnoreCase) && Options.Password.Equals(model.Password))
					{
						return Ok(new { Token = GenerateToken(model.Username)});
					}
					
					return StatusCode(403);
				}
				catch(Exception)
				{
					return StatusCode(500);
				}
			}

			return BadRequest();
		}

		private string GenerateToken(string username)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, username, ClaimValueTypes.String),
				new Claim(ClaimTypes.Role, "Administrator", ClaimValueTypes.String),
				new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
				new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddDays(1)).ToUnixTimeSeconds().ToString())
			};

			var token = new JwtSecurityToken(
				new JwtHeader(
					new SigningCredentials(
						new SymmetricSecurityKey(Encoding.UTF8.GetBytes(Options.SecurityKey)),
						SecurityAlgorithms.HmacSha256)
				),
				new JwtPayload(claims)
			);
					
			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}