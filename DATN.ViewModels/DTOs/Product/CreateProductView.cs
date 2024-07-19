using DATN.ViewModels.DTOs.ProductDetail;

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
        public  List<CreateProductDetaiView> lstPD {get;set;}=new List<CreateProductDetaiView>();
        public string? ImgID { get; set; }
        public string? ImgUrl { get; set; }
    }
}