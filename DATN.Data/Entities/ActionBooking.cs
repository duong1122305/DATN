using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
    public class ActionBooking
    {
        public  int ID { get; set; }
        public string Name { get; set; }
        public virtual IEnumerable<HistoryAction> HistoryActions { get; set; }
    }
}
