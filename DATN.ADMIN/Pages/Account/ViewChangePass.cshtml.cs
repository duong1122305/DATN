using DATN.ADMIN.IServices;
using DATN.ViewModels.DTOs.Authenticate;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DATN.ADMIN.Pages.Account
{
    public class ViewChangePassModel : PageModel
    {
        IHttpContextAccessor _httpContextAccessor;
        IUserClientSev _userClientSev;
        [BindProperty]
        public string password { get; set; }
        [BindProperty]
        public string password_re { get; set; }
        public ViewChangePassModel(IUserClientSev userClientSev, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userClientSev = userClientSev;
        }
        public void OnGet()
        {
            try
            {
                var checkuser = _httpContextAccessor.HttpContext.Session.GetString("username");
                var checkotp = _httpContextAccessor.HttpContext.Session.GetString("otp");
                if (checkuser == null)
                {
                    _httpContextAccessor.HttpContext.Response.Redirect(Url.Content("~/quenMatKhau"));
                }
                else if (checkotp == null)
                {
                    _httpContextAccessor.HttpContext.Response.Redirect(Url.Content("~/OTP"));
                }

            }
            catch (Exception)
            {
                _httpContextAccessor.HttpContext.Response.Redirect(Url.Content("~/dangnhap"));
            }
        }
        public async Task<IActionResult> ResetPass()
        {
            var resetpassview = new UserResetPassView()
            {
                UserName = _httpContextAccessor.HttpContext.Session.GetString("username"),
                NewPassWord = password,
                ConfirmPassWord = password_re,
            };
            var result = await _userClientSev.ResetPass(resetpassview);
            if (result.IsSuccess)
            {
                TempData["e3r"] = null;
                _httpContextAccessor.HttpContext.Session.Remove("username");
                _httpContextAccessor.HttpContext.Session.Remove("otp");
                _httpContextAccessor.HttpContext.Response.Redirect(Url.Content("~/dangnhap"));
            }
            else
            {
                TempData["e3r"] = result.Error;
            }
            return Page();

        }
    }
}
