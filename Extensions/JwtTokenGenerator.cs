using Microsoft.IdentityModel.Tokens;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;

namespace ltbdb.Extensions
{
	public class JwtTokenGenerator
	{
		/// <summary>
		/// Generates a JwtToken.
		/// </summary>
		/// <param name="securityKey">The encryption key.</param>
		/// <param name="username">Issuer username.</param>
		/// <param name="role">Issuer role.</param>
		/// <param name="expire">Expiration time.</param>
		/// <returns>Returns a JwtToken.</returns>
		static public string Generate(string securityKey, string username, string role, int expire = 300)
		{
			var claims = new List<Claim>
			{
				new Claim(ClaimTypes.Name, username, ClaimValueTypes.String),
				new Claim(ClaimTypes.Role, role, ClaimValueTypes.String),
				new Claim(JwtRegisteredClaimNames.Aud, "ltbdb"),
				new Claim(JwtRegisteredClaimNames.Iss, "ltbdb"),
				new Claim(JwtRegisteredClaimNames.Nbf, new DateTimeOffset(DateTime.Now).ToUnixTimeSeconds().ToString()),
				new Claim(JwtRegisteredClaimNames.Exp, new DateTimeOffset(DateTime.Now.AddSeconds(expire)).ToUnixTimeSeconds().ToString())
			};

			var token = new JwtSecurityToken(
				new JwtHeader(
					new SigningCredentials(
						new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)), SecurityAlgorithms.HmacSha256
					)
				),
				new JwtPayload(claims)
			);

			return new JwtSecurityTokenHandler().WriteToken(token);
		}
	}
}