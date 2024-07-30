using DATN.ViewModels.AttributeValidator;
using DATN.ViewModels.DTOs.ProductDetail;
using System.ComponentModel.DataAnnotations;

namespace DATN.ViewModels.DTOs.Product
{
    public class CreateProductView
    {
        public int? Id { get; set; }
		[Required(ErrorMessage = "Phải chọn danh mục")]
		public int IdCategoryProduct { get; set; }
		[Required(ErrorMessage = "Phải có tên sản phẩm")]
		public string Name { get; set; }
		[Required(ErrorMessage = "Phải có mô tả")]
		public string? Description { get; set; }
        public bool Status { get; set; }
		[Required(ErrorMessage = "Phải chọn hãng")]
		public int IdBrand { get; set; }
		public  List<CreateProductDetaiView> lstPD {get;set;}=new List<CreateProductDetaiView>();
		[Required(ErrorMessage = "Phải có ảnh")]
		public string? ImgID { get; set; }
		[Required(ErrorMessage = "Phải có ảnh")]
		public string? ImgUrl { get; set; }
    }
}