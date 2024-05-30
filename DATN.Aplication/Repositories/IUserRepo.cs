using DATN.Data.Entities;
using DATN.ViewModels;
using DATN.ViewModels.ViewModel;

namespace DATN.Aplication.Repositories
{
    public interface IUserRepo
    {
        public Task<string> Login(UserLoginView userLoginView);
        public Task<string> Register(UserRegisterView userRegisterView);
        public Task<string> GenerateTokenString(UserLoginView userLoginView);
        public Task<string> ForgotPassword(string username);
        public Task<List<UserInfView>> GetUsers();
        public Task<User> GetUserAtPhoneNumber(string phonenumber);
    }
}