using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System;

namespace ltbdb.Services
{
	public class TokenService
	{
		/// <summary>
		/// Generates a JwtToken.
		/// </summary>
		/// <param name="securityKey">The encryption key.</param>
		/// <param name="username">Issuer username.</param>
		/// <param name="role">Issuer role.</param>
		/// <param name="expire">Expiration time.</param>
		/// <returns>Returns a JwtToken.</returns>
		public string CreateToken(string securityKey, string username, string role, int expire)
		{
			var _tokenHandler = new JwtSecurityTokenHandler();
			var _key = Encoding.UTF8.GetBytes(securityKey);
			var _tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]{
					new Claim(ClaimTypes.Name, username, ClaimValueTypes.String),
					new Claim(ClaimTypes.Role, role, ClaimValueTypes.String),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				}),
				Audience = "ltbdb",
				Issuer = "ltbdb",
				Expires = DateTime.UtcNow.AddSeconds(expire),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
			};

			var _token = _tokenHandler.CreateToken(_tokenDescriptor);
			return _tokenHandler.WriteToken(_token);
		}
	}
}