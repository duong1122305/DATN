﻿namespace DATN.ViewModels.DTOs.Product
{
    public class ProductView
    {
        public int? Id { get; set; }
        public string CategoryProduct { get; set; }
        public int CategoryProductId { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool Status { get; set; }
        public string Brand { get; set; }
        public int? IdBrand { get; set; }
        public string Price { get; set; }
        public string? Url { get; set; }
        public string? IdImg { get; set; }
    }
}
