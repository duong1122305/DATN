using DATN.ViewModels.Common;

namespace DATN.ViewModels.DTOs.Guest
{
    public class GetGuestResponse
    {
        public PagingResponseData PagingData { get; set; }
        public List<GuestViewModel> lstGuest { get; set; }
    }
}
