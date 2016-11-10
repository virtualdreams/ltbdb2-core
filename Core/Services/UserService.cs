using System.Collections.Generic;
using System.Linq;
using ltbdb.Core.Helper;
using ltbdb.Core.Models;
using Microsoft.Extensions.Logging;
using MongoDB.Bson;
using MongoDB.Driver;

namespace ltbdb.Core.Services
{
    public class UserService: MongoContext
	{
		private readonly ILogger<UserService> Log;

		public UserService(IMongoClient client, ILogger<UserService> logger)
			:base(client)
		{
			Log = logger;
		}

		public IEnumerable<User> Get()
		{
			return User.Find(_ => true).ToEnumerable().OrderBy(o => o.Username).ThenBy(o => o.Role);
		}

		/// <summary>
		/// Get a user by username and password.
		/// </summary>
		/// <param name="username">The username.</param>
		/// <param name="password">The unencrypted password.</param>
		/// <returns>The user object.</returns>
		public User GetUser(string username, string password)
		{
			username = username.Trim();
			password = password.Trim();

			var _filter = Builders<User>.Filter;
			var _username = _filter.Eq(f => f.Username, username);

			if(Log.IsEnabled(LogLevel.Debug))
			{
				User.Find(_username).ToString();
			}

			var _user = User.Find(_username).SingleOrDefault();
			if(_user != null && PasswordHasher.VerifyHashedPassword(_user.Password, password))
				return _user;
			
			return null;
		}

		public ObjectId CreateUser(string username, string password, RoleType role)
		{
			username = username.Trim();
			password = password.Trim();

			var _user = new User
			{
				Username = username,
				Password = PasswordHasher.HashPassword(password),
				Role = role.ToString(),
				Enabled = true
			};

			User.InsertOne(_user);

			if(_user.Id == ObjectId.Empty)
			{
				Log.LogError("Failed to create new user.");
				return ObjectId.Empty;
			}
			else
			{
				Log.LogInformation("Created new user '{0}' -> role '{1}'.", _user.Username, _user.Role);
				return _user.Id;
			}
		}
	}
}