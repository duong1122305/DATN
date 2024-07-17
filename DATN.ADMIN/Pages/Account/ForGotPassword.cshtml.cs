using Azure;
using DATN.ADMIN.IServices;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DATN.ADMIN.Pages.Account
{
    public class ForGotPasswordModel : PageModel
    {
        private readonly IUserClientSev _userClienSev;
        private readonly IHttpContextAccessor _contextAccessor;
        [BindProperty]
        public string email { get; set; }
        public void OnGet()
        {
            if (email == null)
            {
                var checkSpam = _contextAccessor.HttpContext.Session.GetString("username");
                if (checkSpam != null)
                {
                    _contextAccessor.HttpContext.Response.Redirect(Url.Content("~/OTP"));
                }
            }
        }
        public ForGotPasswordModel(IUserClientSev userClient, IHttpContextAccessor httpContextAccessor)
        {
            _userClienSev = userClient;
            _contextAccessor = httpContextAccessor;
        }

        public async Task<IActionResult> CodeOtp()
        {
            try
            {
                if (email != null)
                {
                    var respone = await _userClienSev.ForgotPassword(email);
                    if (respone.IsSuccess)
                    {
                        _contextAccessor.HttpContext.Session.SetString("username", email);
                        _contextAccessor.HttpContext.Response.Redirect(Url.Content("~/OTP"));
                    }
                    else
                    {
                        ModelState.AddModelError("Email", respone.Notifications);
                    }
                }
            }
            catch (Exception e)
            {
                ModelState.AddModelError("Email", $"Đã xảy ra lỗi: {e.Message}");
            }
            return Page();
        }
    }
}
