namespace DATN.ViewModels.DTOs.Authenticate
{
    public class ScheduleView
    {
        public int ShiftID { get; set; }
        public string Shift { get; set; }
        public DateTime WorkDate { get; set; }
        public string Name { get; set; }
        public TimeSpan From { get; set; }
        public TimeSpan To { get; set; }
    }
}
