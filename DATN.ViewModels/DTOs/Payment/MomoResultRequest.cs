using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Payment
{
    public class MomoResultRequest
    {
        public string orderType { get; set; }
        public int amount { get; set; }
        public string partnerCode { get; set; }
        public string orderId { get; set; }
        public string extraData { get; set; }
        public string signature { get; set; }
        public long transId { get; set; }
        public long responseTime { get; set; }
        public int resultCode { get; set; }
        public string message { get; set; }
        public string payType { get; set; }
        public string requestId { get; set; }
        public string orderInfo { get; set; }
    }
}
