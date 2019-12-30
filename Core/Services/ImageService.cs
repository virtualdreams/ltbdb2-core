using Microsoft.Extensions.Logging;
using System;
using System.IO;
using ltbdb.Core.Helpers;

namespace ltbdb.Core.Services
{
	/// <summary>
	/// Type of image to request from storage.
	/// </summary>
	public enum ImageType
	{
		/// <summary>
		/// Get the normal version if available.
		/// </summary>
		Normal,

		/// <summary>
		/// Get the thumbnail version if available.
		/// </summary>
		Thumbnail,

		/// <summary>
		/// Prefer thumbnail over normal version if available.
		/// </summary>
		PreferThumbnail
	}

	public class ImageService
	{
		readonly Settings Options;
		readonly ILogger<ImageService> Log;
		readonly string thumbnailDirectory = "thumb";

		public ImageService(Settings settings, ILogger<ImageService> log)
		{
			Options = settings;
			Log = log;
		}

		/// <summary>
		/// Save image to storage and create a thumbnail.
		/// </summary>
		/// <param name="stream">The image stream.</param>
		/// <param name="createThumbnail">Create also a thumbnail.</param>
		/// <returns>The name of the created file.</returns>
		public string Save(Stream stream, bool createThumbnail = true)
		{
			if (stream == null)
				throw new Exception("Stream must not null.");

			var filename = String.Format("{0}.jpg", GetFilename());
			var imageStorage = GetStoragePath();
			var thumbStorage = GetThumbPath();

			var imagePath = Path.Combine(imageStorage, filename);
			var thumbPath = Path.Combine(thumbStorage, filename);

			Log.LogInformation($"Set image path to '{imagePath}'.");
			Log.LogInformation($"Set thumb path to '{thumbPath}'.");

			GraphicsMagick.GraphicsImage = Options.GraphicsMagick;

			try
			{
				// check if image directory exists, otherwise create it
				if (!Directory.Exists(imageStorage))
					Directory.CreateDirectory(imageStorage);

				using (var output = File.Create(imagePath))
				{
					GraphicsMagick.PInvoke(stream, output, "convert - -background white -flatten jpg:-");
				}

				stream.Position = 0;

				//check if thumbnail directory exists, otherwise create it
				if (!Directory.Exists(thumbStorage))
					Directory.CreateDirectory(thumbStorage);

				using (var output = File.Create(thumbPath))
				{
					GraphicsMagick.PInvoke(stream, output, "convert - -background white -flatten -resize 200x200 jpg:-");
				}

				return filename;
			}
			catch (Exception ex)
			{
				Log.LogError(ex.Message);

				if (File.Exists(imagePath))
				{
					File.Delete(imagePath);
				}

				if (File.Exists(thumbPath))
				{
					File.Delete(thumbPath);
				}

				return null;
			}
		}

		/// <summary>
		/// Test if image exists in store.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="thumbnail">Test for thumbnail.</param>
		/// <returns>True if exists.</returns>
		public bool Exists(string filename, bool thumbnail = false)
		{
			if (String.IsNullOrEmpty(filename))
				return false;

			if (!thumbnail)
			{
				var storage = GetStoragePath();
				var path = Path.Combine(storage, filename);

				return File.Exists(path);
			}
			else
			{
				var storage = GetThumbPath();
				var path = Path.Combine(storage, filename);

				return File.Exists(path);
			}
		}

		/// <summary>
		/// Delete a image from storage.
		/// </summary>
		/// <param name="filename">The filename.</param>
		/// <param name="thumbnail">Delete thumbnail also.</param>
		public void Remove(string filename, bool thumbnail = true)
		{
			if (Exists(filename))
			{
				var storage = GetStoragePath();
				var path = Path.Combine(storage, filename);
				File.Delete(path);

				Log.LogInformation($"Removed file '{path}'.");
			}

			if (Exists(filename, true))
			{
				var storage = GetThumbPath();
				var path = Path.Combine(storage, filename);
				File.Delete(path);

				Log.LogInformation($"Removed thumb file '{path}'.");
			}
		}

		/// <summary>
		/// Get CDN path or return default image.
		/// </summary>
		/// <param name="filename">The image filename.</param>
		/// <param name="imageType">Select type of image to load.</param>
		/// <param name="nullIfEmpty">Return "null" if image not exists.</param>
		/// <returns>The CDN path.</returns>
		public string GetCDNPath(string filename, ImageType imageType = ImageType.Normal, bool nullIfEmpty = false)
		{
			var _cdn = Options.CDNPath;

			switch (imageType)
			{
				case ImageType.Thumbnail:
					if (Exists(filename, true))
						return _cdn.Combine(thumbnailDirectory).Combine(filename);
					goto default;

				case ImageType.PreferThumbnail:
					if (Exists(filename, true))
						return _cdn.Combine(thumbnailDirectory).Combine(filename);

					goto case ImageType.Normal;

				case ImageType.Normal:
					if (Exists(filename))
						return _cdn.Combine(filename);
					goto default;

				default:
					if (!nullIfEmpty)
						return Options.NoImage;
					return null;
			}
		}

		/// <summary>
		/// Generate a new file name from guid.
		/// </summary>
		/// <returns></returns>
		private string GetFilename()
		{
			return Guid.NewGuid().ToString();
		}

		/// <summary>
		/// Get the absolute storage path.
		/// </summary>
		/// <returns></returns>
		private string GetStoragePath()
		{
			return Options.Storage;
		}

		/// <summary>
		/// Get the absolute thumbnail path.
		/// </summary>
		/// <returns></returns>
		private string GetThumbPath()
		{
			return Path.Combine(GetStoragePath(), thumbnailDirectory);
		}
	}
}