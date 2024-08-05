using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Statistical;

namespace DATN.Aplication.Services.IServices
{
    public interface IStatisticalService
    {
		Task<ResponseData<Statistical>> StatisticalIndex(DateTime? startDate, DateTime? endDate, int type = 1);

	}
}