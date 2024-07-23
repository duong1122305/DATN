namespace DATN.Data.Entities
{
    public class Brand
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public virtual ICollection<Product> Products { get; set; }
    }
}
