namespace DATN.ViewModels.DTOs.ServiceDetail
{
    public class GetServiceNameVM
    {
        public int ServiceDetailId { get; set; }
        public string ServiceDetailName { get; set; }
        public string ServiceName { get; set; }
        public float Price { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
