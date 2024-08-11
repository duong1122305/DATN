
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DATN.Aplication;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace DATN.API.Services
{
    public class UploadFileServices : IUploadFileServices
    {
        private readonly Cloudinary _cloundinary;
        private readonly IUnitOfWork _unitOfWork;

        public UploadFileServices(IOptions<CloundinarySettings> cloundConfig, IUnitOfWork unitOfWork)
        {
            Account account = new Account(
                cloundConfig.Value.CloundName,
                cloundConfig.Value.ApiKey,
                cloundConfig.Value.ApiSecret
                );

            _cloundinary = new Cloudinary(account);
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<string>> UploadAvatarAsync(IFormFile file)
        {
            try
            {
                var param = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream())
                };

                var uploadFile = await _cloundinary.UploadAsync(param);

                if (uploadFile.StatusCode == System.Net.HttpStatusCode.OK) // Kiểm tra trạng thái upload
                {
                    return new ResponseData<string>
                    {
                        IsSuccess = true,
                        Data = uploadFile.SecureUrl.AbsoluteUri, // Trả về URL của ảnh
                        Error = null
                    };
                }
                return new ResponseData<string> { IsSuccess = false, Data = null, Error = "Tải ảnh đại diện thất bại" };
            }
            catch (Exception ex)
            {
                return new ResponseData<string> { IsSuccess = false, Data = null, Error = ex.Message };
            }
        }

        public Task<ResponseData<string>> UploadMultipleFileAsync(List<IFormFile> files)
        {
            throw new NotImplementedException();
        }
    }
}
