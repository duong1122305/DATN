namespace DATN.ViewModels.DTOs.Attendace
{
    public class AttendancePersonal
    {
        public string ListShift { get; set; }
        public string ShiftNow { get; set; }
        public string ShiftAttendance { get; set; } = "";
        public int ShiftID { get; set; }
        public int ScheduleID { get; set; }
        /// <summary>
        /// true là chưa check in, false là chưa check out, null là đã thực hiện cả 2
        /// </summary>
        public bool? TypeAttendance { get; set; }
        public string? Note { get; set; }
    }
}
