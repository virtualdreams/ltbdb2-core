using LtbDb.Core.Interfaces;
using LtbDb.Core.Internal;
using LtbDb.Options;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System.IO;
using System;

namespace LtbDb.Core.Services
{
	public class ImageService : IImageService
	{
		private const string GMCommand = "convert - -background white -flatten jpg:-";

		private const string GMThumbnailCommand = "convert - -background white -flatten -resize 200x200 jpg:-";

		private readonly ILogger<ImageService> Log;

		private readonly AppSettings AppSettings;

		private readonly string thumbnailDirectory = "thumb";

		public ImageService(
			ILogger<ImageService> log,
			IOptionsSnapshot<AppSettings> settings)
		{
			AppSettings = settings.Value;
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

			GraphicsMagick.Path = AppSettings.GraphicsMagick;

			try
			{
				// check if image directory exists, otherwise create it
				if (!Directory.Exists(imageStorage))
					Directory.CreateDirectory(imageStorage);

				using (var output = File.Create(imagePath))
				{
					GraphicsMagick.PInvoke(stream, output, GMCommand);
				}

				stream.Position = 0;

				//check if thumbnail directory exists, otherwise create it
				if (!Directory.Exists(thumbStorage))
					Directory.CreateDirectory(thumbStorage);

				using (var output = File.Create(thumbPath))
				{
					GraphicsMagick.PInvoke(stream, output, GMThumbnailCommand);
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
		/// Get image path for or return default image.
		/// </summary>
		/// <param name="filename">The image filename.</param>
		/// <param name="imageType">Select type of image to load.</param>
		/// <param name="nullIfEmpty">Return "null" if image not exists.</param>
		/// <returns>The web image path.</returns>
		public string GetImageWebPath(string filename, ImageType imageType = ImageType.Normal, bool nullIfEmpty = false)
		{
			var _cdn = AppSettings.ImageWebPath;

			Log.LogInformation($"Request image '{filename}' with quality '{imageType}'...");

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
						return AppSettings.DefaultImage;
					return null;
			}
		}

		/// <summary>
		/// Get the physical file path.
		/// </summary>
		/// <param name="filename">The image filename.</param>
		/// <param name="imageType">Image type to load</param>
		/// <returns></returns>
		public string GetPhysicalPath(string filename, ImageType imageType = ImageType.Normal)
		{
			switch (imageType)
			{
				case ImageType.Normal:
					if (Exists(filename, false))
					{
						var imageStorage = GetStoragePath();
						var imagePath = Path.Combine(imageStorage, filename);

						return imagePath;
					}
					goto default;


				case ImageType.Thumbnail:
					if (Exists(filename, true))
					{
						var thumbStorage = GetThumbPath();
						var thumbPath = Path.Combine(thumbStorage, filename);

						return thumbPath;
					}
					goto default;

				default:
					return String.Empty;
			}
		}

		/// <summary>
		/// Get the default image.
		/// </summary>
		/// <param name="nullIfEmpty">Return "null" if image not exists.</param>
		/// <returns>The image path.</returns>
		public string GetDefaultImage(bool nullIfEmpty = false)
		{
			if (!nullIfEmpty)
				return AppSettings.DefaultImage;
			return null;
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
			return Path.GetFullPath(AppSettings.Storage);
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