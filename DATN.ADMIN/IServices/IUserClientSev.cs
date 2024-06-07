using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using System.Collections;

namespace DATN.ADMIN.IServices
{
    public interface IUserClientSev
    {
        Task<ResponseData<List<UserInfView>>> GetAll();

        Task<ResponseData<UserInfView>> GetById(Guid id);
        Task<ResponseData<string>> Login(UserLoginView user);
        Task<UserInfView> UpdateUser(UserInfView userInfView);
        Task<UserInfView> statusUser(DeleteRequest<Guid> deleteRequest);
        Task<ResponseData<UserInfView>> GetInfoUser(string id);
        Task<ResponseData<UserChangePasswordView>> ChangePassword(UserChangePasswordView userChangePasswordView);
    }
}
