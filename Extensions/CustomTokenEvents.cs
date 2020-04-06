using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using System.Threading.Tasks;

namespace ltbdb.Extensions
{
	public class CustomTokenEvents : JwtBearerEvents
	{
		private readonly ILogger<CustomTokenEvents> Log;

		public CustomTokenEvents(ILogger<CustomTokenEvents> log)
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
	}
}