using System.ComponentModel.DataAnnotations;
using MongoDB.Bson;

namespace ltbdb.Models
{
	public class BookModel
	{
		public ObjectId Id { get; set; }

		[Required(ErrorMessage = "Dreck!")]
		public string Title { get; set; }
	}
}