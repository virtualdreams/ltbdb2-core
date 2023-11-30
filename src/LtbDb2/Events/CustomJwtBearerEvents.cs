using LtbDb.Options;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace LtbDb.Events
{
	public class CustomJwtBearerEvents : JwtBearerEvents
	{
		private readonly ILogger<CustomJwtBearerEvents> Log;
		private readonly AppSettings AppSettings;

		public CustomJwtBearerEvents(ILogger<CustomJwtBearerEvents> log, IOptionsSnapshot<AppSettings> settings)
		{
			Log = log;
			AppSettings = settings.Value;
		}

		public override Task AuthenticationFailed(AuthenticationFailedContext context)
		{
			if (context.Exception.GetType() == typeof(SecurityTokenExpiredException))
			{
				Log.LogDebug($"Token expired. Send \"Token-Expired: true\" header.");
				context.Response.Headers["Token-Expired"] = "true";
			}
			return Task.CompletedTask;
		}

		public override Task TokenValidated(TokenValidatedContext context)
		{
			var _principal = context.Principal;
			var _username = _principal.Identity.Name;

			if (!AppSettings.Username.Equals(_username))
			{
				Log.LogInformation($"Benutzername '{_username}' nicht identisch.");
				context.Response.StatusCode = 401;
				context.Fail("Token invalid");
			}

			return Task.CompletedTask;
		}
	}
}