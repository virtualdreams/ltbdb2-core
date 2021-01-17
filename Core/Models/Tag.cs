using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace LtbDb.Core.Models
{
	[Table("tag")]
	public class Tag
	{
		[Column("id")]
		[Required]
		public int Id { get; set; }

		[Column("name")]
		[Required]
		[MaxLength(50)]
		public string Name { get; set; }

		[Column("bookid")]
		[Required]
		public int BookId { get; set; }

		public Book Book { get; set; }
	}
}