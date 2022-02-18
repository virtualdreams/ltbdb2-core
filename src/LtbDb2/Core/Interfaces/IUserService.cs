namespace LtbDb.Core.Interfaces
{
	public interface IUserService
	{
		bool Login(string username, string password);
	}
}