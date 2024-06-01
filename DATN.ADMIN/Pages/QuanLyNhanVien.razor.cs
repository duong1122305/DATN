using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace DATN.ADMIN.Pages
{
    public partial class QuanLyNhanVien
    {
        [Inject] IUserClientSev userClientSev { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        [Parameter] public ResponseData<List<UserInfView>> response { get; set; } = new ResponseData<List<UserInfView>>();
        protected override async Task OnInitializedAsync()
        {
            response = await userClientSev.GetAll();
            if(!response.IsSuccess)
            {
                NavigationManager.NavigateTo("/NotFound");
            }
        }

    }
}
