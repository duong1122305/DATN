using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.ProductDetail
{
    public class CreateProductDetaiView
    {
        public int Id { get; set; }
        public int IdProduct { get; set; }
        public string Name { get; set; }
        public double Price { get; set; }
        public int Amount { get; set; }
      
    }
}
