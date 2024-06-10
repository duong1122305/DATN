using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.Common.Location
{
    public class AddressAPIResponse
    {

        public string error { get; set; }
        public string error_text { get; set; }
        public string data_name { get; set; }
        public List<DataAdress> data { get; set; }
    }
}
