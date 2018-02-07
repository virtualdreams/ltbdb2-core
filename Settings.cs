namespace ltbdb
{
	public class Settings
	{
		/// <summary>
		/// MongoDb connection string.
		/// </summary>
		public string MongoDB { get; set; } = "mongodb://127.0.0.1/";

		/// <summary>
		/// MongoDb database name.
		/// </summary>
		public string Database { get; set; } = "ltbdb";

		/// <summary>
		/// Items per page to display.
		/// </summary>
		public int ItemsPerPage { get; set; } = 18;

		/// <summary>
		/// Recent items to display.
		/// </summary>
		public int RecentItems { get; set; } = 18;

		/// <summary>
		/// Storage path, where the images be saved.
		/// Can be a relative path or a full qualified url.
		/// </summary>
		public string Storage { get; set; } = "./wwwroot/images";

		/// <summary>
		/// Path to image, if no cover exists.
		/// </summary>
		public string NoImage { get; set; } = "/content/no-image.png";

		/// <summary>
		/// CDN path, where the images are located.
		/// </summary>
		public string CDNPath { get; set; } = "/images/";

		/// <summary>
		/// Path to GraphicsMagick to process the uploaded images.
		/// </summary>
		public string GraphicsMagick { get; set; } = "gm";

		/// <summary>
		/// The username to login.
		/// </summary>
		public string Username { get; set; } = "";

		/// <summary>
		/// The password to login.
		/// </summary>
		public string Password { get; set; } = "";
	}
}