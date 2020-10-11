namespace ltbdb.WebAPI.V1.Contracts.Responses
{
	public class RefreshSuccessResponse
	{
		public string AccessToken { get; set; }

		public string Type { get; set; }

		public int ExpiresIn { get; set; }
	}
}