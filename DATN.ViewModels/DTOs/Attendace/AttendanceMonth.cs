namespace DATN.ViewModels.DTOs.Attendace
{
    public class AttendanceMonth
    {
        /// <summary>
        /// ID nhân viên
        /// </summary>
        public Guid IdUser { get; set; }
        /// <summary>
        /// Họ và tên
        /// </summary>
        public string FullName { get; set; } = "";
        /// <summary>
        /// Tên đăng nhập
        /// </summary>
        public string UserName { get; set; } = "";
        /// <summary>
        /// Số ngày làm trên lịch
        /// </summary>
        public int ScheduleWorkingDays { get; set; }
        /// <summary>
        /// Số ngày làm thực tế
        /// </summary>
        public int ActualWorkingDays { get; set; }
        /// <summary>
        /// Số ngày nghỉ
        /// </summary>
        public int TotalLeaveDays { get; set; }
        /// <summary>
        /// Tổng giờ làm
        /// </summary>
        public double TotalWorkingHours { get; set; }
        /// <summary>
        /// Số ngày đi muộn
        /// </summary>
        public int DaysLate { get; set; }
        /// <summary>
        /// Số ngày về sớm
        /// </summary>
        public int DaysLeftEarly { get; set; }

    }
}
