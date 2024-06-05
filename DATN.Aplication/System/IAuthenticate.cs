using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.Aplication.System
{
    public interface IAuthenticate
    {
        public Task<ResponseData<string>> Login(UserLoginView userLoginView);
        public Task<ResponseData<string>> Register(UserRegisterView userRegisterView);
        public Task<ResponseMail> ForgotPassword(string username);
        public Task<ResponseData<List<UserInfView>>> GetUsers();
        public Task<ResponseData<UserInfView>> GetUserAtPhoneNumber(string phonenumber);
        public Task<ResponseData<string>> UpdateInformationUser(UserRegisterView userRegisterView, string id);
        public Task<ResponseData<string>> RemoveUser(string id);
        public Task<ResponseData<string>> ChangePassword(UserChangePasswordView user);
        public Task<ResponseData<string>> GetConfirmCode(string username);
        public Task<ResponseData<bool>> CheckCodeConfirm(string username, string code);
        public Task<ResponseData<string>> GetIdUser(string username);
        public Task<ResponseData<string>> GetRoleUser(string id);
        public Task<ResponseData<string>> AddRoleForUser(AddRoleForUserView addRoleForUserView);
        public Task<ResponseData<List<string>>> ListPosition();
        public Task<ResponseData<string>> AddRole(string roleName);
    }
}