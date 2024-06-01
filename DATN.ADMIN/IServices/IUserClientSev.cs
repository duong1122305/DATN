using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using System.Collections;

namespace DATN.ADMIN.IServices
{
    public interface IUserClientSev
    {
        Task<ResponseData<List<UserInfView>>> GetAll();

        Task<ResponseData<UserInfView>> GetById(Guid id);
        Task<UserInfView> UpdateUser(UserInfView userInfView);
        Task<UserInfView> statusUser(DeleteRequest<Guid> deleteRequest);
    }
}
