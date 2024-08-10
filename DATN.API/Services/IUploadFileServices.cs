using DATN.ViewModels.Common;
using Microsoft.AspNetCore.Cors;

namespace DATN.API.Services
{

    public interface IUploadFileServices
    {
        public Task<ResponseData<string>> UploadAvatarAsync(Guid idUser, IFormFile file);
        public Task<ResponseData<string>> UploadMultipleFileAsync(List<IFormFile> files);
    }
}
