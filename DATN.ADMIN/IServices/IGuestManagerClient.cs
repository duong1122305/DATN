using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Guest;

namespace DATN.ADMIN.IServices
{
    public interface IGuestManagerClient
    {
        Task<ResponseData<List<GuestViewModel>>> GetGuest();
        Task<ResponseData<string>> ChangStatus(bool value, Guid Id);
        Task<ResponseData<string>> RegisterHasUser(GuestRegisterUserRequest request);
        Task<ResponseData<string>> RegisterNoUser(GuestRegisterNoUserRequest request);
        Task<ResponseData<string>> UpdateGuets(GuestUpdateRequest request);


    }
}