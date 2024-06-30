using Azure;
using DATN.ADMIN.IServices;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DATN.ADMIN.Pages.Account
{
    public class ViewOTPModel : PageModel
    {
        [BindProperty]
        public string code { get; set; }
        IHttpContextAccessor _httpContextAccessor { get; set; }
        IUserClientSev _userClientSev { get; set; }

        public ViewOTPModel(IUserClientSev userClientSev, IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
            _userClientSev = userClientSev;
        }
        public void OnGet()
        {
            if (code == null)
            {
                var checkSpam = _httpContextAccessor.HttpContext.Session.GetString("otp");
                if (checkSpam != null)
                {
                    _httpContextAccessor.HttpContext.Response.Redirect(Url.Content("~/changePass"));
                }
            }
        }
        public async Task<IActionResult> CheckOtp()
        {
            try
            {
               
                    if (code != null)
                    {
                        var username = _httpContextAccessor.HttpContext.Session.GetString("username");
                        var result = await _userClientSev.CheckCodeOtp(username, code);
                        if (result.IsSuccess)
                        {
                        TempData["er"] = null ;
                        _httpContextAccessor.HttpContext.Session.SetString("otp", code);
                            _httpContextAccessor.HttpContext.Response.Redirect(Url.Content("~/changePass"));
                    }
                    else
                        {
                            TempData["er"] = result.Error;
                            return RedirectToPage("/OTP");
                        }
                    }
                
            }
            catch (Exception)
            {

            }
            return Page();
        }
    }
}
