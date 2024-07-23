namespace DATN.ViewModels.DTOs.Authenticate
{
    public class UserUpdateView
    {
        public string FullName { get; set; }
        public string? Address { get; set; }
        public string PhoneNumber { get; set; }
        public string? Email { get; set; }
        public bool? Gender { get; set; }
        public DateTime? DoB { get; set; }
    }
}
