using LtbDb.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LtbDb.Core.Interfaces
{
	public interface IBookService
	{
		Task<IList<Book>> GetAsync();
		Task<Book> GetByIdAsync(int id);
		Task<IList<Book>> GetByCategoryAsync(string category);
		Task<IList<Book>> GetByTagAsync(string tag);
		Task<IList<Book>> GetByFilterAsync(string category, string tag);
		Task<IList<Book>> GetRecentlyAddedAsync(int limit);
		Task<Book> CreateAsync(Book book);
		Task UpdateAsync(Book book);
		Task DeleteAsync(int id);
		Task SetImageAsync(int id, Stream stream);
	}
}