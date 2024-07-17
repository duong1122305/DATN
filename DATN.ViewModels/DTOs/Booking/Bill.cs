using DATN.ViewModels.DTOs.Product;
using DATN.ViewModels.DTOs.ServiceDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.ViewModels.DTOs.Booking
{
    public class Bill
    {
        public string GuestName { get; set; }
        public string PhoneNumber { get; set; }
        public string Address { get; set; }
        public  double TotalPrice { get; set; }
        public DateTime DateBooking { get; set; }
        public int? IdVoucher { get; set; }
        public double ReducePrice { get; set; }
        public double TotalPayment { get; set; }
        public List<ServiceDetailView> ListServiceBooked { get; set; }
        public List<ProductDetailView> ListProductDetail { get; set; }
    }
}
