using DATN.ViewModels.Common;
using Microsoft.AspNetCore.Http;

namespace DATN.API.Services
{

    public interface IUploadFileServices
    {
        public Task<ResponseData<string>> UploadAvatarAsync(IFormFile file);
        public Task<ResponseData<string>> UploadMultipleFileAsync(List<IFormFile> files);
    }
}
