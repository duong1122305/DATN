
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using DATN.Aplication;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
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
        public async Task<ResponseData<string>> UploadAvatarAsync(Guid idUser, IFormFile file)
        {
            try
            {
                var guest = await _unitOfWork.GuestRepository.GetAsync(idUser);
                if (guest == null)
                {
                    return new ResponseData<string> { IsSuccess = false, Data = null, Error = "Có lỗi trong quá trình xác thực tài khoản của bạn" };
                }

                var param = new ImageUploadParams()
                {
                    File = new FileDescription(file.FileName, file.OpenReadStream())
                };

                var uploadFile = await _cloundinary.UploadAsync(param);

                guest.AvatarUrl = uploadFile.SecureUrl.AbsoluteUri;

                await _unitOfWork.GuestRepository.UpdateAsync(guest);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ResponseData<string> { IsSuccess = true, Data = "Tải ảnh đại diện thành công", Error = null };
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
