﻿namespace DATN.Data.Entities
{
    public class CategoryDetails
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int IdCategory { get; set; }
        public bool IsDeleted { get; set; }
        public virtual ICollection<Product> Products { get; set; }
        public virtual Category Category { get; set; }
    }
}
