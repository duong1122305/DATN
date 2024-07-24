namespace DATN.ViewModels.DTOs.Attendace
{
    public class AttendanceViewModel
    {
        public int ID { get; set; }
        public string UserName { get; set; }
        public string StaffName { get; set; }
        public int ScheduleID { get; set; }
        public string DateAttendace { get; set; }
        public string CheckInTime { get; set; }
        public string CheckOutTime { get; set; }

    }
}
