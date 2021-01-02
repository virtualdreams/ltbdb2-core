using System.Collections.Generic;
using System.Threading.Tasks;

namespace ltbdb.Core.Interfaces
{
	public interface ITagService
	{
		Task<List<string>> GetAsync();
		Task<List<string>> SuggestionsAsync(string term);
	}
}