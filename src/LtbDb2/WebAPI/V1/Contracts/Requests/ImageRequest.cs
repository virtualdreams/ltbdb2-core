using Microsoft.AspNetCore.Http;

namespace LtbDb.WebAPI.V1.Contracts.Requests
{
	public class ImageRequest
	{
		public IFormFile Image { get; set; }
	}
}