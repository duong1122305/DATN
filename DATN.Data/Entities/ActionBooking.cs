namespace DATN.Data.Entities
{
    public class ActionBooking
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<HistoryAction> HistoryActions { get; set; }
    }
}
