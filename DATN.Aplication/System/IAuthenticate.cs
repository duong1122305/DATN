using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Request.Authenticate;

namespace DATN.Aplication.System
{
    public interface IAuthenticate
    {
        public Task<ResponseData<string>> Login(UserLoginView userLoginView);
        public Task<string> Register(UserRegisterView userRegisterView);
        public Task<string> GenerateTokenString(UserLoginView userLoginView);
        public Task<ResponseMail> ForgotPassword(string username);
        public Task<List<UserInfView>> GetUsers();
        public Task<User> GetUserAtPhoneNumber(string phonenumber);
        public Task<bool> UpdateInformationUser(UserRegisterView userRegisterView);
        public Task<string> ChangePassword(UserChangePasswordView user);
        public Task<string> GetConfirmCode(string username);
        public Task<bool> CheckCodeConfirm(string username, string code);
    }
}