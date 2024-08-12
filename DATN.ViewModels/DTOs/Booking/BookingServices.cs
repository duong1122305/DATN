using DATN.ViewModels.DTOs.Product;

namespace DATN.ViewModels.DTOs.Booking
{
    public class BookingService
    {
        public List<CreateBookingDetailRequest> lstBooking { get; set; } = new List<CreateBookingDetailRequest>();
        public List<ProductDetailView> ListProductDetail { get; set; } = new List<ProductDetailView>();

        public event Action OnChange;

        public void AddBooking(List<CreateBookingDetailRequest> booking)
        {
            lstBooking.AddRange(booking);
            NotifyStateChanged();
        } 

        public void AddProduct(ProductDetailView product)
        {
            if (product.Term != 0)
            {
                var check = ListProductDetail.FirstOrDefault(c => c.IdProductDetail == product.IdProductDetail && c.Term == product.Term);
                if (check != null)
                {
                    check.SelectQuantityProduct++;
                }
                else
                {
                    ListProductDetail.Add(product);
                    NotifyStateChanged();
                }
            }
            else
            {
                var check = ListProductDetail.FirstOrDefault(c => c.IdProductDetail == product.IdProductDetail && c.IdBooking == product.IdBooking && c.IdBooking != 0);
                if (check != null)
                {
                    check.SelectQuantityProduct++;
                }
                else
                {
                    ListProductDetail.Add(product);
                    NotifyStateChanged();
                }
            }
        }

        public void RemoveProduct(ProductDetailView product)
        {
            ListProductDetail.Remove(product); // Xoá đối tượng booking từ danh sách
        }
        public void RemoveBillTerm(List<ProductDetailView> products)
        {
            foreach (var item in products)
            {
                ListProductDetail.Remove(item);
            } // Xoá đối tượng booking từ danh sách
        }

        public void RemoveBooking(CreateBookingDetailRequest booking)
        {
            if (booking != null)
            {
                lstBooking.Remove(booking); // Xoá đối tượng booking từ danh sách
                //NotifyStateChanged(); // Thông báo sự thay đổi để giao diện có thể cập nhật
            }
        }
        public void RemoveBookingAll(CreateBookingDetailRequest booking)
        {
            lstBooking.Remove(booking);
        }
        private void NotifyStateChanged()
        {
            OnChange?.Invoke();
        }
        public void RemoveProductAll(ProductDetailView product)
        {
            ListProductDetail.Remove(product);
        }

    }
}
