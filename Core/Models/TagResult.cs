using MongoDB.Bson.Serialization.Attributes;

namespace ltbdb.Core.Models
{
	public class TagResult
	{
		[BsonId]
		public string Id { get; set; }
	}
}