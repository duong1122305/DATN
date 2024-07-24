namespace DATN.ViewModels.DTOs.Authenticate
{
    public class ChangeShiftView
    {
        public int ShiftId { get; set; }
        public string UserIdFirst { get; set; }
        public string UserIdSecond { get; set; }
        public DateTime Date { get; set; }
    }
}
