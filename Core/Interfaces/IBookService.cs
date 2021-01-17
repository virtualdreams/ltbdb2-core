using LtbDb.Core.Models;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace LtbDb.Core.Interfaces
{
	public interface IBookService
	{
		Task<List<Book>> GetAsync();
		Task<Book> GetByIdAsync(int id);
		Task<List<Book>> GetByCategoryAsync(string category);
		Task<List<Book>> GetByTagAsync(string tag);
		Task<List<Book>> GetByFilterAsync(string category, string tag);
		Task<List<Book>> GetRecentlyAddedAsync(int limit);
		Task<Book> CreateAsync(Book book);
		Task UpdateAsync(Book book);
		Task DeleteAsync(int id);
		Task SetImageAsync(int id, Stream stream);
	}
}