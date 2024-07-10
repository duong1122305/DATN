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
        public Task<ResponseData<string>> UpdateInformationUser(UserUpdateView userUpdateView, string id);
        public Task<ResponseData<string>> RemoveUser(string id);
        public Task<ResponseData<string>> ChangePassword(UserChangePasswordView user);
        public Task<ResponseData<string>> ResetPassword(UserResetPassView user);
        public Task<ResponseData<string>> CheckCodeConfirm(string username, string code);
        public Task<ResponseData<string>> GetIdUser(string username);
        public Task<ResponseData<string>> GetRoleUser(string id);
        public Task<ResponseData<string>> AddRoleForUser(AddRoleForUserView addRoleForUserView);
        public Task<ResponseData<List<RoleView>>> ListPosition();
        public Task<ResponseData<string>> AddRole(string roleName);
        public Task<ResponseData<string>> ActiveAccount(string id);
        public Task<ResponseData<UserInfView>> GetInfById(string id);
        public Task<ResponseData<string>> GetUserByToken(string token);
    }
}