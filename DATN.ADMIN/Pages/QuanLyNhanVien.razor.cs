using DATN.ADMIN.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.Reflection.Metadata;

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
        // Biến kiểm soát hiển thị popup
        private bool showPopup = false;

        // Phương thức mở popup
        private void OpenPopup()
        {
            showPopup = true;
        }

        // Phương thức đóng popup
        private void ClosePopup()
        {
            showPopup = false;
        }

        private bool confirmAction = false;

        private void CheckboxClicked()
        {
            // Xử lý khi checkbox được chọn
        }

        private void ActionTypeChanged(string value)
        {
            // Xử lý khi người dùng chọn loại hành động
            if (value == "change-status")
            {
                // Hiển thị combobox chọn ca làm
            }
        }

        private void QuickActionApplyClicked()
        {
            // Xử lý khi người dùng ấn nút "Đồng ý"
            // Hiển thị modal xác nhận hành động
            confirmAction = true;
        }

        private void ConfirmAction()
        {
            // Xử lý hành động xác nhận
            // Đóng modal và thực hiện hành động tương ứng
            confirmAction = false;
        }
    }
}
