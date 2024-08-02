using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Statistical;

namespace DATN.ADMIN.IServices
{
	public interface IStatiscalClient
	{
		Task<ResponseData<Statistical>> StatisticalIndex( DateTime? startDate, DateTime? endDate,int type = 1);

	}
}
