using DATN.Utilites.Validator;

namespace DATN.ViewModels.DTOs.Guest
{
    public class GuestChangPassRequest
    {
        public string UserName { get; set; }
        [PasswordValidation]
        public string OldPass { get; set; }
        [PasswordValidation]
        public string NewPass { get; set; }
    }
}
