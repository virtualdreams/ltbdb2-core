using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using ltbdb.Extensions;

namespace ltbdb.Models
{
	public class BookModel
	{
		/// <summary>
		/// The book id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The book number.
		/// </summary>
		public int? Number { get; set; }

		/// <summary>
		/// The book title.
		/// </summary>
		public string Title { get; set; }

		/// <summary>
		/// The category of the book.
		/// </summary>
		public string Category { get; set; }

		/// <summary>
		/// The book creation date and time.
		/// </summary>
		public DateTime Created { get; set; }

		/// <summary>
		/// The image filename.
		/// </summary>
		public string Filename { get; set; }

		private string[] _stories = new string[] { };

		/// <summary>
		/// The stories in the book.
		/// </summary>
		public string[] Stories
		{
			get
			{
				return _stories;
			}
			set
			{
				if (value != null)
				{
					_stories = value;
				}
			}
		}

		private string[] _tags = new string[] { };

		/// <summary>
		/// The tags for the book.
		/// </summary>
		public string[] Tags
		{
			get
			{
				return _tags;
			}
			set
			{
				if (value != null)
				{
					_tags = value;
				}
			}
		}
	}

	public class BookPostModel
	{
		/// <summary>
		/// The book id.
		/// </summary>
		public int Id { get; set; }

		/// <summary>
		/// The book number.
		/// </summary>
		[Required(ErrorMessage = "Bitte gib eine Nummer ein.")]
		public int? Number { get; set; }

		/// <summary>
		/// The book title.
		/// </summary>
		[Required(ErrorMessage = "Bitte gib einen Titel ein.")]
		[MaxLength(100, ErrorMessage = "Der Titel darf max. 100 Zeichen lang sein.")]
		public string Title { get; set; }

		/// <summary>
		/// The category of the book.
		/// </summary>
		[Required(ErrorMessage = "Bitte gib eine Kategorie ein.")]
		[MaxLength(100, ErrorMessage = "Die Kategorie darf max. 100 Zeichen lang sein.")]
		public string Category { get; set; }

		/// <summary>
		/// The image filename.
		/// </summary>
		public string Filename { get; set; }

		private string[] _stories = new string[] { };

		/// <summary>
		/// The stories in the book.
		/// </summary>
		[ArrayItemMaxLength(100, ErrorMessage = "Ein Eintrag darf max. 100 Zeichen lang sein.")]
		public string[] Stories
		{
			get
			{
				return _stories;
			}
			set
			{
				if (value != null)
				{
					_stories = value;
				}
			}
		}

		/// <summary>
		/// The tags for the book.
		/// </summary>
		[StringArrayItemMaxLength(50, ErrorMessage = "Ein Tag darf max. 50 Zeichen lang sein.")]
		public string Tags { get; set; }


		/// <summary>
		/// The posted image.
		/// </summary>
		public IFormFile Image { get; set; }

		/// <summary>
		/// Delete image flag.
		/// </summary>
		public bool Remove { get; set; }
	}

	public class BookPostApiModel
	{
		/// <summary>
		/// The book number.
		/// </summary>
		[Required(ErrorMessage = "Bitte gib eine Nummer ein.")]
		public int? Number { get; set; }

		/// <summary>
		/// The book title.
		/// </summary>
		[Required(ErrorMessage = "Bitte gib einen Titel ein.")]
		[MaxLength(100, ErrorMessage = "Der Titel darf max. 100 Zeichen lang sein.")]
		public string Title { get; set; }

		/// <summary>
		/// The category of the book.
		/// </summary>
		[Required(ErrorMessage = "Bitte gib eine Kategorie ein.")]
		[MaxLength(100, ErrorMessage = "Die Kategorie darf max. 100 Zeichen lang sein.")]
		public string Category { get; set; }

		private string[] _stories = new string[] { };

		/// <summary>
		/// The stories in the book.
		/// </summary>
		[ArrayItemMaxLength(100, ErrorMessage = "Der Inhalt darf max. 100 Zeichen lang sein.")]
		public string[] Stories
		{
			get
			{
				return _stories;
			}
			set
			{
				if (value != null)
				{
					_stories = value;
				}
			}
		}

		public string[] _tags = new string[] { };

		/// <summary>
		/// The tags for the book.
		/// </summary>
		public string[] Tags
		{
			get
			{
				return _tags;
			}
			set
			{
				if (value != null)
				{
					_tags = value;
				}
			}
		}
	}
}