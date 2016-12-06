namespace ltbdb
{
	public class Settings
	{
		/// <summary>
		/// MongoDb connection string.
		/// </summary>
		public string MongoDB { get; set; }

		/// <summary>
		/// MongoDb database name.
		/// </summary>
		public string Database { get; set; }

		/// <summary>
		/// Items per page to display.
		/// </summary>
		public int ItemsPerPage { get; set; }

		/// <summary>
		/// Recent items to display.
		/// </summary>
		public int RecentItems { get; set; }

		/// <summary>
		/// Storage path, where the images be saved.
		/// Can be a relative path or a full qualified url.
		/// </summary>
		public string Storage { get; set; }

		/// <summary>
		/// Path to image, if no cover exists.
		/// </summary>
		public string NoImage { get; set; }

		/// <summary>
		/// CDN path, where the images are located.
		/// </summary>
		public string CDNPath { get; set; }

		/// <summary>
		/// Path to GraphicsMagick to process the uploaded images.
		/// </summary>
		public string GraphicsMagick { get; set; }

		/// <summary>
		/// Use mongodb to read login data instead of the config file.
		/// </summary>
		public bool UseDatabaseAuthentication { get; set; }

		/// <summary>
		/// The username to login.
		/// </summary>
		public string Username { get; set; }

		/// <summary>
		/// The password to login.
		/// </summary>
		public string Password { get; set; }
	}
}