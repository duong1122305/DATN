namespace DATN.ViewModels.DTOs.Attendace
{
    public class ListPerAttenMonth
    {
        public AttendanceMonth AllData { get; set; }
        public List<AttendancePerMonth> AttendancePerMonths { get; set; } = new List<AttendancePerMonth>();
    }
}
