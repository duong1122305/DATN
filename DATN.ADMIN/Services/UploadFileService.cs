using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DATN.ADMIN.IServices;
using DATN.Aplication;
using DATN.Data.Entities;
using DATN.Utilites;
using DATN.ViewModels.Common;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Options;
using System.Net;

namespace DATN.ADMIN.Services
{
	public class UploadFileService : IUpLoadFileService
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
		public async Task<ResponseData<string>> UploadFile(IFormFile file)
		{
			try
			{
				var param = new ImageUploadParams()
				{
					File = new FileDescription(file.FileName, file.OpenReadStream())
				};

				var uploadFile = await _cloundinary.UploadAsync(param);
				var x = uploadFile.StatusCode;
				var result = uploadFile.SecureUrl.AbsoluteUri;


				return new ResponseData<string>("ok");
			}
			catch (Exception ex)
			{
				return new ResponseData<string>(false, "Thất bại" + ex); ;
			}
		}
		
		public async Task<ResponseData<string[]>> UploadFile(IBrowserFile file)
		{
			try
			{
				if (file.Size > Contant.MaxIMGSize)
				{
					return new ResponseData<string[]>(false, "File chỉ nhận dưới 5mb");
				}
				string[] imageFormats = { "jpg", "png", "gif", "bmp", "tif","webp","jpeg" };
				if (!imageFormats.Contains(file.Name.Split('.').Last()))
				{
					return new ResponseData<string[]>(false, "Hãy chọn file ảnh");
				}
				var param = new ImageUploadParams()
				{
					File = new FileDescription(Guid.NewGuid().ToString(), file.OpenReadStream(Contant.MaxIMGSize))
				};

				var uploadFile = await _cloundinary.UploadAsync(param);

				var response = uploadFile.StatusCode;
                if (response==HttpStatusCode.OK)
                {
					var url = uploadFile.SecureUrl.AbsoluteUri;
					var publicId = uploadFile.PublicId;
				    return new ResponseData<string[]>(new string[] { url, publicId });
				}
				return new ResponseData<string[]>(false, "Thêm không thành công");
			}
			catch (Exception ex)
			{
				return new ResponseData<string[]>(false, "Có lỗi gì đó");
			}
		}
		public async Task<ResponseData<string>> RemoveImg(string publicId)
		{
			var deleteParams = new DeletionParams(publicId);

			var result = await _cloundinary.DestroyAsync(deleteParams);

			if (result.StatusCode == System.Net.HttpStatusCode.NoContent)
			{
				return new ResponseData<string>("Xoá thành công");
			}
			else
			{ 
				return new ResponseData<string>(false, "Xoá thất bại");
			}

		}

	}
}
