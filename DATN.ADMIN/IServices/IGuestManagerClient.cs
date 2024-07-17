using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Guest;

namespace DATN.ADMIN.IServices
{
    public interface IGuestManagerClient
    {
        Task<ResponseData<List<GuestViewModel>>> GetGuest();
        Task<ResponseData<string>> ChangStatus(bool value, Guid Id);
        Task<ResponseData<string>> RegisterNoUser(GuestRegisterByGuestRequest request);
        Task<ResponseData<string>> UpdateGuets(GuestUpdateRequest request);


    }
}