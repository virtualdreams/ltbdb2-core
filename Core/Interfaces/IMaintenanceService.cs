using System.Threading.Tasks;
using ltbdb.Core.Models;

namespace ltbdb.Core.Interfaces
{
	public interface IMaintenanceService
	{
		Task<Statistic> GetStatisticsAsync();
	}
}