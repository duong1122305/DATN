using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.Aplication.System
{
    public interface IAuthenticateGuest
    {
        Task<ResponseData<string>> Login(UserLoginView request);
        Task<ResponseData<string>> ChangePass(string userName, string oldPass, string newPass);


    }
}
