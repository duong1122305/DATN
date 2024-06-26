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
        [BindProperty(SupportsGet = true)]
        public string password { get; set; }
        [BindProperty(SupportsGet = true)]
        public string password_re { get; set; }
        public ViewChangePassModel(IUserClientSev userClientSev, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userClientSev = userClientSev;
        }
        public async Task ResetPass()
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
                _httpContextAccessor.HttpContext.Session.Remove("username");
                _httpContextAccessor.HttpContext.Session.Remove("otp");
                _httpContextAccessor.HttpContext.Response.Redirect(Url.Content("~/dangnhap"));
            }
        }
    }
}
