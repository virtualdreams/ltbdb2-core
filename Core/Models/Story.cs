using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace ltbdb.Core.Models
{
	[Table("story")]
	public class Story
	{
		[Column("id")]
		[Required]
		public int Id { get; set; }

		[Column("name")]
		[Required]
		[MaxLength(200)]
		public string Name { get; set; }

		[Column("bookid")]
		[Required]
		public int BookId { get; set; }

		public Book Book { get; set; }
	}
}