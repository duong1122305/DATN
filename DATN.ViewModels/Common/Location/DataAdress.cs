using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.Common.Location
{
    public class DataAdress
    {
        public string id { get; set; }
        public string longitude { get; set; }
        public string latitude { get; set; }
        public string full_name_en { get; set; }
        public string full_name { get; set; }
        public string name_en { get; set; }
        public string name { get; set; }
        public string? tinh { get; set; }
        public string? quan { get; set; }
    }
}
