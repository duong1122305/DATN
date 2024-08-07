﻿using DATN.ADMIN.IServices;
using DATN.ViewModels.DTOs.Authenticate;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DATN.ADMIN.Pages.Account
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

                    if (userLoginView.UserName != null && userLoginView.Password != null)
                    {
                        var checkLogin = _userClienSev.Login(userLoginView).GetAwaiter().GetResult();

                        if (checkLogin.IsSuccess && checkLogin.Data != null)
                        {

                            _contextAccessor.HttpContext.Session.SetString("Key", checkLogin.Data);
                            _contextAccessor.HttpContext.Response.Redirect(Url.Content("~/trangchu"));

                        }
                        else if (!checkLogin.IsSuccess)
                        {
                            TempData["Error"] = checkLogin.Error;
                        }
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }

    }
}
