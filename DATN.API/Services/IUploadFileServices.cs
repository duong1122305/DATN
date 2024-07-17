using DATN.ViewModels.Common;

namespace DATN.API.Services
{
    public interface IUploadFileServices
    {
        public Task<ResponseData<string>> UploadAvatarAsync(Guid idUser, IFormFile file);
        public Task<ResponseData<string>> UploadMultipleFileAsync(List<IFormFile> files);
    }
}
