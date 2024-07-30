namespace DATN.ViewModels.DTOs.Payment
{
    public class DataPayment
    {
        public int acpId { get; set; }
        public string accountName { get; set; }
        public string qrCode { get; set; }
        public string qrDataURL { get; set; }
    }
}
