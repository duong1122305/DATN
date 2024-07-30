namespace DATN.Data.Enum
{
    public enum BookingStatus
    {
        PendingConfirmation, // Chờ xác nhận
        Confirmed,           // Đã xác nhận
        Arrived,             // Đã đến nơi
        InProgress,          // Đang thực hiện
        Completed,           // Đã hoàn thành
        NoShow,              // Khách không đến
        CustomerCancelled,   // Khách huỷ
        StaffCancelled,      // Nhân viên huỷ
        AdminCancelled       // Admin huỷ
    }
}
