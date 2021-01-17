using LtbDb.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LtbDb.Core.Interfaces
{
	public interface ISearchService
	{
		Task<List<Book>> SearchAsync(string term);
		Task<List<string>> SearchSuggestionsAsync(string term);
		Task<List<string>> CategorySuggestionsAsync(string term);
		Task<List<string>> TagSuggestionsAsync(string term);
	}
}