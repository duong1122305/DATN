namespace DATN.ViewModels.DTOs.Authenticate
{
    public class UserInfView
    {
        public string UserName { get; set; }
        public string FullName { get; set; }
        public string? Password { get; set; }
        public string PhoneNumber { get; set; }
        public string Gender { get; set; }
        public string Address { get; set; }
        public string Email { get; set; }
        public string Position { get; set; }
        public bool IsDeleted { get; set; }
        public bool IsConfirm { get; set; }
        public string? ImgUrl { get; set; }
        public string? ImgId { get; set; }
    }
}
