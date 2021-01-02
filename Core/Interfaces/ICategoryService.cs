using System.Collections.Generic;
using System.Threading.Tasks;

namespace ltbdb.Core.Interfaces
{
	public interface ICategoryService
	{
		Task<List<string>> GetAsync();
		Task RenameAsync(string from, string to);
		Task<List<string>> SuggestionsAsync(string term);
	}
}