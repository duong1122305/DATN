namespace DATN.Data.Entities
{
    public class HistoryAction
    {
        public int ID { get; set; }
        public int ActionID { get; set; }
        public int BookingID { get; set; }
        public bool ByGuest { get; set; }
        public Guid? ActionByID { get; set; }
        public DateTime ActionTime { get; set; }
        public string  Description { get; set; }
        public virtual ActionBooking Action { get; set; }
        public virtual Booking Booking { get; set; }
        public virtual User ActionBy { get; set; }
    }
}