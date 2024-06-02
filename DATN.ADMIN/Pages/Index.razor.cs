using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

namespace DATN.ADMIN.Pages
{
    public partial class Index
    {
        [Inject]
        AuthenticationStateProvider authenticationStateProvider {  get; set; }
        private bool isAuthorized;

        protected override async Task OnInitializedAsync()
        {
            var authState = await authenticationStateProvider.GetAuthenticationStateAsync();
            isAuthorized = authState.User.Identity.IsAuthenticated;
        }
    }
}
