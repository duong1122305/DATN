using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Statistical;

namespace DATN.ADMIN.IServices
{
	public interface IStatiscalClient
	{
		Task<ResponseData<Statistical>> StatisticalIndex( string? startDate, string? endDate,int type = 1);

	}
}
