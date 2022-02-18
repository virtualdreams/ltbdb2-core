using LtbDb.Core.Interfaces;
using LtbDb.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;

namespace LtbDb.Core.Services
{
	public class UserService : IUserService
	{
		private readonly ILogger<UserService> Log;
		private readonly AppSettings AppSettings;

		public UserService(ILogger<UserService> log, IOptionsSnapshot<AppSettings> settings)
		{
			Log = log;
			AppSettings = settings.Value;
		}

		/// <summary>
		/// Check login data against config.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The password.</param>
		public bool Login(string username, string password)
		{
			if (String.IsNullOrEmpty(AppSettings.Username) || String.IsNullOrEmpty(AppSettings.Password))
			{
				return false;
			}

			if (AppSettings.Username.Equals(username, StringComparison.OrdinalIgnoreCase) && AppSettings.Password.Equals(password))
			{
				return true;
			}

			return false;
		}
	}
}