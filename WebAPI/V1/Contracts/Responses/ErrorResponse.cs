using System.Collections.Generic;

namespace ltbdb.WebAPI.V1.Contracts.Responses
{
	public class ErrorResponse
	{
		public string Field { get; set; }

		public List<string> Messages { get; set; } = new List<string>();
	}
}