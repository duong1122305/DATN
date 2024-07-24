using DATN.ViewModels.Common;

namespace DATN.ViewModels.DTOs.Request.Service
{
    public class PagingServiceRequest : PagingRequestBase
    {
        public string? keyWord { get; set; }
    }
}
