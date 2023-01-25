using System.Collections.Generic;
using System.Threading.Tasks;

namespace LtbDb.Core.Interfaces
{
	public interface ICategoryService
	{
		Task<IList<string>> GetAsync();
		Task RenameAsync(string from, string to);
	}
}