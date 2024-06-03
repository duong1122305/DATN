using DATN.ADMIN.IServices;
using DATN.ADMIN.Services;
using DATN.Data.Entities;
using DATN.ViewModels.DTOs.Authenticate;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System.Security.Claims;

namespace DATN.ADMIN.Pages
{
    public class LoginModel : PageModel
    {
        private readonly IUserClientSev _userClienSev;
        UserLoginView userLoginView { get; set; }
        [BindProperty]
        public string email { get; set; }
        [BindProperty]
        public string password { get; set; }
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        HttpContextAccessor _contextAccessor;

        public LoginModel(SignInManager<User> signInManager, UserManager<User> userManager, IUserClientSev userClientSev, HttpContextAccessor contextAccessor)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userClienSev = userClientSev;
            _contextAccessor = contextAccessor;
        }

        public async Task HandleLogin()
        {
            try
            {
                userLoginView = new UserLoginView()
                {
                    UserName = email,
                    Password = password
                };
                var user = _userClienSev.Login(userLoginView).GetAwaiter().GetResult();

                if (user.IsSuccess && user.Data != null)
                {
                    _contextAccessor.HttpContext.Session.SetString("key",user.Data);
                    _contextAccessor.HttpContext.Response.Redirect(Url.Content("~/"));
                }

            }
            catch (Exception e)
            {
                _contextAccessor.HttpContext.Response.Redirect(Url.Content("~/dangnhap"));
            }
        }
    }
}
