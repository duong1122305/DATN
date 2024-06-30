using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.ProductDetail
{
    public class ProductDetaiView
    {
        public int Id { get; set; }
        public string Product { get; set; }
        public int ProductId { get; set; }
        public string PetType { get; set; }
        public int PetTypeId { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
        public bool IsDeleted { get; set; }
    }
}
