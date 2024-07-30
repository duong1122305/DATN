namespace DATN.Data.Entities
{
    public class Product
    {
        public int Id { get; set; }
        public int IdCategoryProduct { get; set; }// IDCategoryDetails
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public int IdBrand { get; set; }
        public virtual Brand Brands { get; set; }
        public virtual ICollection<ProductDetail> ProductDetails { get; set; }
        public virtual ICollection<ImageProduct> ImageProducts { get; set; }
        public virtual CategoryDetails CategoryDetails { get; set; }

    }
}
