using CloudinaryDotNet.Actions;
using DATN.ViewModels.Common;
using Microsoft.AspNetCore.Components.Forms;

namespace DATN.ADMIN.IServices
{
    public interface IUpLoadFileService
    {
		Task<ResponseData<string>> UploadFile(IFormFile file);
		Task<ResponseData<string[]>> UploadFile(IBrowserFile file);
		Task<ResponseData<string>> RemoveImg(string publicId);
	}
}