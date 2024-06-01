using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using Microsoft.AspNetCore.Components;

namespace DATN.ADMIN.Pages.ManagentStaff
{
    public partial class CalendarViewSstaff
    {
        [Inject] IEmployeeScheduleSer scheduleSer { get; set; }
        [Inject] NavigationManager NavigationManager { get; set; }

        [Parameter] public ResponseData<List<ScheduleView>> response { get; set; } = new ResponseData<List<ScheduleView>>();
        protected async Task OnInitializedAsync()
        {
            response = await scheduleSer.GetAll();
            if (!response.IsSuccess)
            {
                NavigationManager.NavigateTo("/NotFound");
            }
        }
    }
}
