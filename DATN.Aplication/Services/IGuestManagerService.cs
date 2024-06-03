using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Guest;

namespace DATN.Aplication.Services
{
    public interface IGuestManagerService
    {
        Task<ResponseData<string>> RegisterWithUser(GuestRegisterUserRequest request);
        Task<ResponseData<string>> RegisterNoUser(GuestRegisterNoUserRequest request);
        Task<ResponseData<string>> RegisterByCustomer(GuestRegisterUserRequest request);
        Task<ResponseData<string>>UpdateGuest(GuestUpdateRequest request);
        Task<ResponseData<GetGuestResponse>>GetGuest(GetGuestRequest request);
        Task<ResponseData<GuestViewModel>>FindGuestByID(Guid id);
        Task<ResponseData<string>> VerififyUser(string verifyToken, string mail);
        Task<ResponseData<string>> SoftDelete(DeleteRequest<Guid> request);
    }
}