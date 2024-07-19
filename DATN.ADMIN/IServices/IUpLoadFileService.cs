using CloudinaryDotNet.Actions;
using DATN.ViewModels.Common;
using Microsoft.AspNetCore.Components.Forms;

namespace DATN.ADMIN.IServices
{
    public interface IUpLoadFileService
    {
		Task<ResponseData<string>> UploadFile(IFormFile file);
		/// <summary>
		/// Post file lên cờ lao đin nà ri
		/// </summary>
		/// <param name="file"> Ném file vào đây</param>
		/// <returns> Data trả về mảng 2 phần tử [0] là url [1] là publicID</returns>
		Task<ResponseData<string[]>> UploadFile(IBrowserFile file);
		Task<ResponseData<string>> RemoveImg(string publicId);
	}
}