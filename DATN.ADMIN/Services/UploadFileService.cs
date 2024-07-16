using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DATN.ADMIN.IServices;
using DATN.Aplication;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using Microsoft.Extensions.Options;

namespace DATN.ADMIN.Services
{
    public class UploadFileService: IUpLoadFileService
	{
		private readonly Cloudinary _cloundinary;
		private readonly IUnitOfWork _unitOfWork;

		public UploadFileService(IOptions<CloundinarySettings> cloundConfig, IUnitOfWork unitOfWork)
		{
			Account account = new Account(
				cloundConfig.Value.CloundName,
				cloundConfig.Value.ApiKey,
				cloundConfig.Value.ApiSecret
				);

			_cloundinary = new Cloudinary(account);
			_unitOfWork = unitOfWork;
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
	}
}
