using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Statistical;

namespace DATN.ADMIN.IServices
{
	public interface IStatiscalClient
	{
		Task<ResponseData<Statistical>> StatisticalIndex(int type = 1);

	}
}
