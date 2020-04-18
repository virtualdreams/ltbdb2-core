namespace ltbdb.WebAPI.V1.Contracts.Responses
{
	public class AuthSuccessResponse
	{
		public string Token { get; set; }

		public string Type { get; set; }

		public int ExpiresIn { get; set; }
	}
}