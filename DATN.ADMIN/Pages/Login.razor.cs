using Microsoft.AspNetCore.Components;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.ADMIN.Pages
{
    public partial class Login
    {
        [Inject]
        UserLoginView user { get; set; } = new UserLoginView();

        HttpClient client { get; set; }
        NavigationManager navigationManager { get; set; }


        public async Task Submit()
        {
            var response = await client.PostAsJsonAsync("api/UserLogin/User-Login", user);

            if(response.IsSuccessStatusCode)
            {
                navigationManager.NavigateTo("/trangchu");
            }
            else
            {
                navigationManager.NavigateTo("/login");
            }
        }
    }
}
