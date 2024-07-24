namespace DATN.ViewModels.DTOs.Attendace
{
    public class ShiftVM
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public TimeSpan? Start { get; set; }
        public TimeSpan? End { get; set; }
        public bool IsCheckin { get; set; }
        public bool? IsLate { get; set; }
    }
}
