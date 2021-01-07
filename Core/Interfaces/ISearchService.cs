using System.Collections.Generic;
using System.Threading.Tasks;
using ltbdb.Core.Models;

namespace ltbdb.Core.Interfaces
{
	public interface ISearchService
	{
		Task<List<Book>> SearchAsync(string term);
		Task<List<string>> SearchSuggestionsAsync(string term);
		Task<List<string>> CategorySuggestionsAsync(string term);
		Task<List<string>> TagSuggestionsAsync(string term);
	}
}