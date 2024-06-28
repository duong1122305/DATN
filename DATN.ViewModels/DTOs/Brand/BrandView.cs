using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Brand
{
    public class BrandView
    {
        public int? Id { get; set; }
        public string Name { get; set; }
        public string? Description { get; set; }
        public bool? Status { get; set; }
    }
}
