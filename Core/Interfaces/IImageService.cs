using System.IO;

namespace LtbDb.Core.Interfaces
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

	public interface IImageService
	{
		string Save(Stream stream, bool createThumbnail = true);
		bool Exists(string filename, bool thumbnail = false);
		void Remove(string filename, bool thumbnail = true);
		string GetImageWebPath(string filename, ImageType imageType = ImageType.Normal, bool nullIfEmpty = false);
		string GetPhysicalPath(string filename, ImageType imageType = ImageType.Normal);
		string GetDefaultImage(bool nullIfEmpty = false);
	}
}