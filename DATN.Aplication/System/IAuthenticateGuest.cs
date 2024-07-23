using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.Aplication.System
{
    public interface IAuthenticateGuest
    {
        Task<ResponseData<string>> Login(UserLoginView request);


    }
}
