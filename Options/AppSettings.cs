//using System.ComponentModel.DataAnnotations;

namespace ltbdb.Options
{
	public class AppSettings
	{
		public const string AppSettingsName = "Settings";

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
		public string DefaultImage { get; set; } = "/content/no-image.png";

		/// <summary>
		/// Web path, where the images are located.
		/// </summary>
		public string ImageWebPath { get; set; } = "/images/";

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

		/// <summary>
		/// Path to keystore directory.
		/// </summary>
		public string KeyStore { get; set; } = "";

		/// <summary>
		/// JWT access token key.
		/// </summary>
		//[MinLength(16)]
		public string AccessTokenKey { get; set; } = "";

		/// <summary>
		/// JWT access token expire in minutes.
		/// </summary>
		public int AccessTokenExpire { get; set; } = 5;
	}
}