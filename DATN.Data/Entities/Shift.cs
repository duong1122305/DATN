using System.ComponentModel.DataAnnotations.Schema;

namespace DATN.Data.Entities
{
    [Table("Shift")]// Ca làm việc
    public class Shift
    {
        public int Id { get; set; } // PK

        public string Name { get; set; } // Tên hoặc mã của ca làm việc

        public TimeSpan From { get; set; } // Thời gian bắt đầu ca

        public TimeSpan To { get; set; } // Thời gian kết thúc ca

        // Navigation property
        public virtual ICollection<WorkShift> WorkShifts { get; set; }
    }
}