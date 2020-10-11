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
		/// Generates a access token.
		/// </summary>
		/// <param name="securityKey">The encryption key.</param>
		/// <param name="username">Issuer username.</param>
		/// <param name="role">Issuer role.</param>
		/// <param name="expire">Expiration time.</param>
		/// <returns>Returns a access token.</returns>
		public string CreateAccessToken(string securityKey, string username, string role, int expire)
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
				Expires = DateTime.UtcNow.AddMinutes(expire),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
			};

			var _token = _tokenHandler.CreateToken(_tokenDescriptor);
			return _tokenHandler.WriteToken(_token);
		}

		/// <summary>
		/// Get data from refresh token.
		/// </summary>
		/// <param name="token">The access token.</param>
		/// <param name="securityKey">The encryption key.</param>
		/// <returns>Returns principal claims.</returns>
		public ClaimsPrincipal GetPrincipalFromRefreshToken(string token, string securityKey)
		{
			var _tokenHandler = new JwtSecurityTokenHandler();
			var _tokenValidationParameters = new TokenValidationParameters
			{
				ValidateAudience = true,
				ValidAudience = "ltbdb-refresh",
				ValidateIssuer = true,
				ValidIssuer = "ltbdb",
				ValidateLifetime = false,
				IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(securityKey)),
			};

			SecurityToken _securityToken;
			var _principal = _tokenHandler.ValidateToken(token, _tokenValidationParameters, out _securityToken);
			// var jwtSecurityToken = _securityToken as JwtSecurityToken;

			var _name = _principal.Identity.Name;
			var _role = _principal.FindFirst(ClaimTypes.Role)?.Value;

			return _principal;
		}

		/// <summary>
		/// Generates a refresh token.
		/// </summary>
		/// <param name="securityKey">The encryption key.</param>
		/// <param name="username">Issuer username</param>
		/// <returns>Returns a JWT</returns>
		public string CreateRefreshToken(string securityKey, string username, string tokenId)
		{
			var _tokenHandler = new JwtSecurityTokenHandler();
			var _key = Encoding.UTF8.GetBytes(securityKey);
			var _tokenDescriptor = new SecurityTokenDescriptor
			{
				Subject = new ClaimsIdentity(new[]{
					new Claim(ClaimTypes.Name, username, ClaimValueTypes.String),
					new Claim("xid", tokenId),
					new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
				}),
				Audience = "ltbdb-refresh",
				Issuer = "ltbdb",
				Expires = DateTime.UtcNow.AddDays(7),
				SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_key), SecurityAlgorithms.HmacSha256Signature)
			};

			var _token = _tokenHandler.CreateToken(_tokenDescriptor);
			return _tokenHandler.WriteToken(_token);
		}
	}
}