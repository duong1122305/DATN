using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using DATN.ViewModels.DTOs.Authenticate;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Components;


namespace DATN.ADMIN.Pages
{
    public class LoginModel : PageModel
    {
        HttpClient _http;
        NavigationManager _navigation;
        public LoginModel(HttpClient http, NavigationManager navigation)
        {
            _http = http;
            _navigation = navigation;
        }
        public void OnGet()
        {
        }

        public async Task Login(string username, string password)
        {
            UserLoginView userLoginView = new UserLoginView
            {
                UserName = username,
                Password = password
            };

            var login = await _http.PostAsJsonAsync("api/UserLogin/User-Login", userLoginView);

            if(login.IsSuccessStatusCode)
            {
                _navigation.NavigateTo("/");
            }
        }
    }
}
