using LtbDb.Options;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.Threading.Tasks;

namespace LtbDb.Events
{
	public class CustomCookieAuthenticationEvents : CookieAuthenticationEvents
	{
		private readonly ILogger<CustomCookieAuthenticationEvents> Log;
		private readonly AppSettings AppSettings;

		public CustomCookieAuthenticationEvents(ILogger<CustomCookieAuthenticationEvents> log, IOptionsSnapshot<AppSettings> settings)
		{
			Log = log;
			AppSettings = settings.Value;
		}

		public override async Task ValidatePrincipal(CookieValidatePrincipalContext context)
		{
			var _principal = context.Principal;
			var _username = _principal.Identity.Name;

			if (!AppSettings.Username.Equals(_username))
			{
				Log.LogInformation($"Benutzername '{ _username}' nicht identisch.");
				context.RejectPrincipal();
				await context.HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
			}
		}
	}
}