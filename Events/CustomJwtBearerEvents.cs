using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ltbdb.Events
{
	public class CustomJwtBearerEvents : JwtBearerEvents
	{
		private readonly ILogger<CustomJwtBearerEvents> Log;

		public CustomJwtBearerEvents(ILogger<CustomJwtBearerEvents> log)
		{
			Log = log;
		}

		public override Task AuthenticationFailed(AuthenticationFailedContext context)
		{
			if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
			{
				Log.LogDebug($"Token expired. Send \"Token-Expired: true\" header.");
				context.Response.Headers.Add("Token-Expired", "true");
			}
			return Task.CompletedTask;
		}

		/* public override Task TokenValidated(TokenValidatedContext context)
		{
			var _context = context.Principal;
			var _name = _context.FindFirst(ClaimTypes.Name)?.Value;

			if (_name.Equals("test"))
			{
				context.Response.StatusCode = 401;
				context.Fail("Token invalid");
			}

			return Task.CompletedTask;
		} */
	}
}