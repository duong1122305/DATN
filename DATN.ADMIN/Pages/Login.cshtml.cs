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
        NavigationManager Navigation { get; set; }
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;

        public LoginModel(SignInManager<User> signInManager, UserManager<User> userManager, IUserClientSev userClientSev)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _userClienSev = userClientSev;
        }

        public async Task<IActionResult> HandleLogin()
        {
            try
            {
                userLoginView = new UserLoginView()
                {
                    UserName = email,
                    Password = password
                };
                var user = _userClienSev.Login(userLoginView).GetAwaiter().GetResult();
                if (user.IsSuccess)
                {
                    return RedirectToPage("/trangchu");
                }
                else
                    return RedirectToPage("/dang-nhap");

            }
            catch (Exception)
            {
                return RedirectToPage("/dang-nhap");

            }
        }
    }
}
