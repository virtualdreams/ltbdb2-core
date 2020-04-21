using Microsoft.AspNetCore.Http;

namespace ltbdb.WebAPI.V1.Contracts.Requests
{
	public class ImageRequest
	{
		public IFormFile Image { get; set; }
	}
}