using DATN.ADMIN.IServices;
using DATN.ADMIN.Services;
using DATN.Aplication.CustomProvider;
using DATN.Data.Entities;
using DATN.ViewModels.DTOs.Authenticate;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
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
        HttpContextAccessor _contextAccessor;
        public LoginModel(IUserClientSev userClientSev, HttpContextAccessor contextAccessor)
        {
            _userClienSev = userClientSev;
            _contextAccessor = contextAccessor;
        }
        public async Task HandleLogin()
        {
            if (_contextAccessor.HttpContext.Session.GetString("Key") != null)
            {
                _contextAccessor.HttpContext.Response.Redirect(Url.Content("~/trangchu"));
            }
            else
            {
                try
                {
                    userLoginView = new UserLoginView()
                    {
                        UserName = email,
                        Password = password
                    };
                    var checkLogin = _userClienSev.Login(userLoginView).GetAwaiter().GetResult();

                    if (checkLogin.IsSuccess && checkLogin.Data != null)
                    {
                        _contextAccessor.HttpContext.Session.SetString("Key", checkLogin.Data);
                        _contextAccessor.HttpContext.Response.Redirect(Url.Content("~/trangchu"));
                    }

                }
                catch (Exception e)
                {
                    _contextAccessor.HttpContext.Response.Redirect(Url.Content("~/dangnhap"));
                }
            }
        }
    }
}
