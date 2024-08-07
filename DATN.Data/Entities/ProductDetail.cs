﻿using DATN.ViewModels.Enum;

namespace DATN.Data.Entities
{
    public class ProductDetail
    {
        public int Id { get; set; }
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public int AmountUsed { get; set; }
        public ProductDetailStatus Status { get; set; }
        public virtual Product Product { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }

    }
}
