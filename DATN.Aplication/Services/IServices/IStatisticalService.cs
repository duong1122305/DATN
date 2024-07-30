using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Statistical;

namespace DATN.Aplication.Services.IServices
{
    public interface IStatisticalService
    {
		Task<ResponseData<Statistical>> StatisticalIndex(int type = 1);

	}
}