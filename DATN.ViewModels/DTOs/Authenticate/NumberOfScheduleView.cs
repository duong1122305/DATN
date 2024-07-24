namespace DATN.ViewModels.DTOs.Authenticate
{
    public class NumberOfScheduleView
    {
        public Guid? IdStaff { get; set; }
        public int shiftId { get; set; }
        public string ShiftName { get; set; }
        public string UserName { get; set; }
        public string FullName { get; set; }
        public DateTime Date { get; set; }
    }
}
