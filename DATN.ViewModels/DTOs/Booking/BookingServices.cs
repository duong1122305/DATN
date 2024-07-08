using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
    public class BookingService
    {
        public List<CreateBookingDetailRequest> lstBooking { get; set; } = new List<CreateBookingDetailRequest>();
        public event Action OnChange;
        public void AddBooking(CreateBookingDetailRequest booking)
        {
            lstBooking.Add(booking);
            NotifyStateChanged();
        }

        public void RemoveBooking(CreateBookingDetailRequest booking)
        {
            var bookingToRemove = lstBooking.FirstOrDefault(b => b.BookingId == booking.BookingId); // Tìm đối tượng booking cần xoá
            if (bookingToRemove != null)
            {
                lstBooking.Remove(bookingToRemove); // Xoá đối tượng booking từ danh sách
                NotifyStateChanged(); // Thông báo sự thay đổi để giao diện có thể cập nhật
            }
        }
        public void RemoveBookingAll(CreateBookingDetailRequest booking)
        {
            lstBooking.Remove(booking);
        }
        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }

    }
}
