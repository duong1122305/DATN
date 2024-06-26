using DATN.ADMIN.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DATN.ADMIN.Pages.Account
{
    public class ForGotPassworrdModel : PageModel
    {
        private readonly IUserClientSev _userClienSev;
        [BindProperty]
        public string email { get; set; }
        public void OnGet()
        {
        }
        public ForGotPassworrdModel(IUserClientSev userClient)
        {
            _userClienSev = userClient;
        }

        public async Task CodeOtp()
        {
            var respone = await _userClienSev.ForgotPassword(email);
            if (respone.IsSuccess)
            {
                
            }
        }
    }
}
