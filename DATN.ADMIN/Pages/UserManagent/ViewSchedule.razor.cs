namespace DATN.ADMIN.Pages.UserManagent
{
    public partial class ViewSchedule
    {
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
    }
}
