namespace DATN.ViewModels.Common
{
    public class ResponseMail
    {
        public bool IsSuccess { get; set; }
        public string? Error { get; set; }
        public string Notifications { get; set; }
    }
}
