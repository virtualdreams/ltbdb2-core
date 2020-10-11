using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace ltbdb.Events
{
	public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
	{
		private readonly ILogger<CustomCookieAuthenticationEvents> Log;
		private readonly Settings Options;

		public CustomCookieAuthenticationEvents(ILogger<CustomCookieAuthenticationEvents> log, IOptionsSnapshot<Settings> settings)
		{
			Log = log;
			Options = settings.Value;
		}

		public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
		{
			var _principal = context.Principal;
			var _username = _principal.Identity.Name;

			if (!Options.Username.Equals(_username))
			{
				Log.LogInformation($"Benutzername '{ _username}' nicht identisch.");
				context.RejectPrincipal();
				await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			}
		}
	}
}