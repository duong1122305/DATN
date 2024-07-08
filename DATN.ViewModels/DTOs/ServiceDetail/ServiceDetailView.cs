using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.ServiceDetail
{
    public class ServiceDetailView
    {
        public int IdServiceDetail { get; set; }
        public double Price { get; set; }
        public string NameStaff { get; set; }
        public string PetName { get; set; }
    }
}
