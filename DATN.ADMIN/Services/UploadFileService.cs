using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DATN.ADMIN.IServices;
using DATN.Aplication;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;

namespace DATN.ADMIN.Services
{
    public class UploadFileService: IUpLoadFileService
	{
		private readonly Cloudinary _cloundinary;

		public UploadFileService(IOptions<CloundinarySettings> cloundConfig)
		{
			Account account = new Account(
				cloundConfig.Value.CloundName,
				cloundConfig.Value.ApiKey,
				cloundConfig.Value.ApiSecret
				);

			_cloundinary = new Cloudinary(account);
		}
		public async Task<ResponseData<string>> UploadFile( IFormFile file)
		{
			try
			{
				var param = new ImageUploadParams()
				{
					File = new FileDescription(file.FileName, file.OpenReadStream())
				};

				var uploadFile = await _cloundinary.UploadAsync(param);
				var x= uploadFile.StatusCode;
				var result=  uploadFile.SecureUrl.AbsoluteUri;


				return new ResponseData<string>("ok");
			}
			catch (Exception ex)
			{
				return  new ResponseData<string>(false,"Thất bại"+ex); ;
			}
		}
		public async Task<ResponseData<string>> UploadFile( IBrowserFile file)
		{
			try
			{
				var param = new ImageUploadParams()
				{
					File = new FileDescription(Guid.NewGuid().ToString(), file.OpenReadStream())
				};

				var uploadFile = await _cloundinary.UploadAsync(param);

				var x= uploadFile.StatusCode;
				var result=  uploadFile.SecureUrl.AbsoluteUri;


				return new ResponseData<string>("ok");
			}
			catch (Exception ex)
			{
				return  new ResponseData<string>(false,"Thất bại"+ex); ;
			}
		}
		public async Task<DeletionResult> RemoveImg(string publicId)
		{
			var deleteParams = new DeletionParams(publicId);

			var result = await _cloundinary.DestroyAsync(deleteParams);

			return result;
		}

	}
}
