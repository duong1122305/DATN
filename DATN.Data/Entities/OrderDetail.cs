using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Data.Entities
{
    public class OrderDetail
    {
        public int Id { get; set; }
        public int IdBooking { get; set; }
        public int IdProductDetail { get; set; }
        public double Price { get; set; }
        public int Quantity { get; set; }
        public virtual Booking Booking { get; set; }
        public virtual ProductDetail ProductDetail { get; set; }
    }
}
