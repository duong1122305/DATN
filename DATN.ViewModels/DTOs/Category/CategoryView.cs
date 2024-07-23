namespace DATN.ViewModels.DTOs.Category
{
    public class CategoryView
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool? IsDeleted { get; set; }
    }
}
