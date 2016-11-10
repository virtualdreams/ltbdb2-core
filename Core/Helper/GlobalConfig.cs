using Singleton;
using ConfigFile;
using Microsoft.Extensions.Logging;

namespace ltbdb.Core.Helpers
{
    public sealed class GlobalConfig: SingletonBase<GlobalConfig>
	{
		private readonly ILogger<GlobalConfig> Log = Startup.Logger.CreateLogger<GlobalConfig>();
		
		private GlobalConfig()
		{
			ReadConfiguration();
		}

		public static GlobalConfig Get()
		{
			return GetInstance();
		}
		
		#region Public members

		/// <summary>
		/// Kestrel server address.
		/// </summary>
		/// <returns></returns>
		public string Kestrel { get; private set; }

		/// <summary>
		/// MongoDB connection string.
		/// </summary>
		public string MongoDB { get; private set; }
		
		/// <summary>
		/// Items per page to display.
		/// </summary>
		public int ItemsPerPage { get; private set; }

		/// <summary>
		/// Recent items to display.
		/// </summary>
		public int RecentItems { get; private set; }
		
		/// <summary>
		/// The username to login.
		/// </summary>
		public string Username { get; private set; }
		
		/// <summary>
		/// The password to login.
		/// </summary>
		public string Password { get; private set; }
		
		/// <summary>
		/// Use mongodb to read login data instead of the config file.
		/// </summary>
		public bool UseDatabaseAuthentication { get; set; }

		/// <summary>
		/// Storage path, where the images be saved.
		/// Can be a relative path or a full qualified url.
		/// </summary>
		public string Storage { get; private set; }

		/// <summary>
		/// CDN path, where the images are located.
		/// </summary>
		public string CDNPath { get; private set; }

		/// <summary>
		/// Path to image, if no cover exists.
		/// </summary>
		public string NoImage { get; private set; }
		
		/// <summary>
		/// Path to GraphicsMagick to process the uploaded images.
		/// </summary>
		public string GraphicsMagick { get; private set; }

		#endregion
		
		#region Read configuration
		private void ReadConfiguration()
		{
			Log.LogInformation("Load configuration...");

			var config = new ConfigReader("./Config/application.conf");

			this.Kestrel = config.GetValue<string>("kestrel", "http://*:5000", true);
			//Log.InfoFormat("Set kestrel address to {0}.", this.Kestrel);

			this.MongoDB = config.GetValue<string>("mongodb", "mongodb://127.0.0.1/", true);
			Log.LogInformation("Set mongodb connection string to {0}.", this.MongoDB);

			this.ItemsPerPage = config.GetValue<int>("items_per_page", 18, true);
			Log.LogInformation("Set items per page to {0}.", this.ItemsPerPage);

			this.RecentItems = config.GetValue<int>("recent_items", 18, true);
			Log.LogInformation("Set recently added items to {0}.", this.RecentItems);

			this.Storage = config.TryGetValue<string>("storage", true);
			Log.LogInformation("Set storage path to {0}.", this.Storage);

			this.CDNPath = config.TryGetValue<string>("cdn", true);
			Log.LogInformation("Set cdn to {0}.", this.CDNPath);

			this.NoImage = config.GetValue<string>("no_image", "");
			Log.LogInformation("Set no image path to {0}.", this.NoImage);

			this.GraphicsMagick = config.GetValue<string>("gm", "gm", true);
			Log.LogInformation("Set graphics magick executable to {0}.", this.GraphicsMagick);

			this.Username = config.TryGetValue<string>("username", true);
			this.Password = config.TryGetValue<string>("password", true);

			this.UseDatabaseAuthentication = config.GetValue<bool>("usedbauth", false, true);
			Log.LogInformation("Use database authentication {0}.", this.UseDatabaseAuthentication);

			Log.LogInformation("Load configuration finished.");
		}
		#endregion
	}
}
