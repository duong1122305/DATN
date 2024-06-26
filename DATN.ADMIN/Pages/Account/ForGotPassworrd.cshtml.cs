using DATN.ADMIN.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DATN.ADMIN.Pages.Account
{
    public class ForGotPassworrdModel : PageModel
    {
        private readonly IUserClientSev _userClienSev;
        private readonly IHttpContextAccessor _contextAccessor;
        [BindProperty(SupportsGet = true)]
        public string email { get; set; }
        public void OnGet()
        {
        }
        public ForGotPassworrdModel(IUserClientSev userClient, IHttpContextAccessor httpContextAccessor)
        {
            _userClienSev = userClient;
            _contextAccessor = httpContextAccessor;
        }

        public async Task CodeOtp()
        {
            var respone = await _userClienSev.ForgotPassword(email);
            if (respone.IsSuccess)
            {
                _contextAccessor.HttpContext.Session.SetString("username", email);
                _contextAccessor.HttpContext.Response.Redirect(Url.Content("~/OTP"));
            }
        }
    }
}
