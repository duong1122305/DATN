namespace DATN.ViewModels.DTOs.Product
{
    public class CreateProductView
    {
        public int? Id { get; set; }
        public int IdCategoryProduct { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public int IdBrand { get; set; }
    }
}