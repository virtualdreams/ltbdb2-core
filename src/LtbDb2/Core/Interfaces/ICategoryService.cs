using System.Collections.Generic;
using System.Threading.Tasks;

namespace LtbDb.Core.Interfaces
{
	public interface ICategoryService
	{
		Task<List<string>> GetAsync();
		Task RenameAsync(string from, string to);
	}
}