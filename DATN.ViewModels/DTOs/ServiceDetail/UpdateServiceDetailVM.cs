using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.ServiceDetail
{
    public class UpdateServiceDetailVM
    {
        public string ServiceDetailName { get; set; }
        public float Price { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
    }
}
