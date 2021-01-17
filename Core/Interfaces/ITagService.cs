using System.Collections.Generic;
using System.Threading.Tasks;

namespace LtbDb.Core.Interfaces
{
	public interface ITagService
	{
		Task<List<string>> GetAsync();
	}
}