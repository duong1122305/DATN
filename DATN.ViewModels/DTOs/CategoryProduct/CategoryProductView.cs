namespace DATN.ViewModels.DTOs.CategoryProduct
{
    public class CategoryProductView
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int CategoryId { get; set; }
        public bool IsDeleted { get; set; }
    }
}
