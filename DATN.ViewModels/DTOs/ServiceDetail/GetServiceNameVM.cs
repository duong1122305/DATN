using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.ServiceDetail
{
    public class GetServiceNameVM
    {
        public string ServiceDetailName { get; set; }
        public string ServiceName { get; set; }
        public float Price { get; set; }
        public double Duration { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }
        public bool IsDeleted { get; set; }
    }
}
