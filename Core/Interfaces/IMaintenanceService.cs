using LtbDb.Core.Models;
using System.Threading.Tasks;

namespace LtbDb.Core.Interfaces
{
	public interface IMaintenanceService
	{
		Task<Statistic> GetStatisticsAsync();
	}
}