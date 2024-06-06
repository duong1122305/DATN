using DATN.Aplication.CustomProvider;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;

namespace DATN.ADMIN.Pages
{
    public partial class Index
    {
        [Inject]
        protected IHttpContextAccessor _httpContextAccessor { get; set; }
        [Inject]
        protected CustomAuthenticationStateProvider _customAuthenticationStateProvider { get; set; }
        protected override async Task OnInitializedAsync()
        {
            try
            {
                var key = _httpContextAccessor.HttpContext.Session.GetString("Key");
                if (key != null)
                {
                    var authState = await _customAuthenticationStateProvider.GetAuthenticationStateAsync();
                    var user = authState.User.Identity.IsAuthenticated;
                }
            }
            catch (Exception)
            {

                throw;
            }
        }
    }
}
