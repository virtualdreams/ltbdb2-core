using LtbDb.Core.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LtbDb.Core.Interfaces
{
	public interface ISearchService
	{
		Task<IList<Book>> SearchAsync(string term);
		Task<IList<string>> SearchSuggestionsAsync(string term);
		Task<IList<string>> CategorySuggestionsAsync(string term);
		Task<IList<string>> TagSuggestionsAsync(string term);
	}
}