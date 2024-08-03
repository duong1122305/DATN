using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.ServiceDetailVM
{
    public class GroupByServiceName
    {
        public string ServiceName { get; set; }
        public List<GetServiceDetail> ServiceDetails { get; set; }
    }

    public class GetServiceDetail
    {
        public int Id { get; set; }
        public string Description { get; set; }
        public double Price { get; set; }
        public double Duration { get; set; }
        public bool IsActive { get; set; }
    }
}
