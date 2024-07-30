namespace DATN.ViewModels.DTOs.Attendace
{
    public class AttendancePerMonth
    {
        public string Date { get; set; }
        public string Shift { get; set; }
        public string TimeOfShift { get; set; }
        public string CheckinAt { get; set; }
        public string CheckoutAt { get; set; }
        public string History { get; set; } = "";
    }
}
