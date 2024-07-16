using DATN.ViewModels.Common;

namespace DATN.ADMIN.IServices
{
    public interface IUpLoadFileService
    {
		Task<ResponseData<string>> UploadFile(IFormFile file);

	}
}