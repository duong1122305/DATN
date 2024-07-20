using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Product;
using DATN.ViewModels.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
    public class ProductManagement : IProductManagement
    {
        IUnitOfWork _unitOfWork;
        public ProductManagement(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<string>> BuyProduct(List<BuyProduct> buyProducts)
        {
            try
            {
                List<OrderDetail> orderDetails = new List<OrderDetail>();
                foreach (var buyProduct in buyProducts)
                {
                    OrderDetail orderDetail = new OrderDetail()
                    {
                        IdBooking = buyProduct.IdBooking,
                        IdProductDetail = buyProduct.IdProductDetail,
                        Price = buyProduct.Price,
                        Quantity = buyProduct.Quantity,
                    };
                    orderDetails.Add(orderDetail);
                }
                await _unitOfWork.OrderDetailRepository.AddRangeAsync(orderDetails);
                return new ResponseData<string> { IsSuccess = true, Data = "Thành công" };
            }
            catch (Exception e)
            {
                return new ResponseData<string> { IsSuccess = false, Error = e.Message };
            }
        }
        public async Task<ResponseData<BillProduct>> GetBillProduct(int id)
        {
            try
            {
                var query = from order in await _unitOfWork.OrderDetailRepository.GetAllAsync()
                            join productDetail in await _unitOfWork.ProductDetailRepository.GetAllAsync()
                            on order.IdProductDetail equals productDetail.Id
                            where order.IdBooking == id
                            select new ProductDetailView
                            {
                                Name = productDetail.Name,
                                Price = order.Price,
                                Quantity = order.Quantity,
                            };
                BillProduct billProduct = new BillProduct()
                {
                    ListProductDetail = query.ToList(),
                };
                foreach (var item in query)
                {
                    billProduct.TotalPrice += item.Price * item.Quantity;
                }
                return new ResponseData<BillProduct> { IsSuccess = true, Data = billProduct };
            }
            catch (Exception e)
            {
                return new ResponseData<BillProduct> { IsSuccess = false, Error = e.Message };
            }
        }
        public async Task CheckStatusProduct()
        {
            var queryDetail = await _unitOfWork.ProductDetailRepository.GetAllAsync();
            var queryOrder = await _unitOfWork.OrderDetailRepository.GetAllAsync();
            if (queryDetail.Count() > 0 && queryOrder.Count() > 0)
            {
                List<ProductDetail> lstSuccess = new List<ProductDetail>();
                foreach (var item in queryDetail)
                {
                    var count = 0;
                    foreach (var item1 in queryOrder)
                    {
                        if (item.Id == item1.IdProductDetail)
                        {
                            count += item1.Quantity;
                        }
                    }
                    if (item.Amount >= count)
                    {
                        item.Status = ProductDetailStatus.OutOfStock;
                        lstSuccess.Add(item);
                    }
                }
                await _unitOfWork.ProductDetailRepository.UpdateRangeAsync(lstSuccess);
            }
        }
        public async Task<ResponseData<List<ProductSelect>>> ListProductViewSale()
        {
            var queryDetail = await _unitOfWork.ProductDetailRepository.GetAllAsync();
            await CheckStatusProduct();
            var query = from product in await _unitOfWork.ProductRepository.GetAllAsync()
                        join brand in await _unitOfWork.BrandRepository.GetAllAsync()
                        on product.IdBrand equals brand.Id
                        join cate in await _unitOfWork.CategoryDetailRepository.GetAllAsync()
                        on product.IdCategoryProduct equals cate.Id
                        join img in await _unitOfWork.ImageProductRepository.GetAllAsync()
                        on product.Id equals img.ProductID
                        where product.Status == true
                        group new { product, brand, cate, img }
                        by new { product, brand, cate, img }
                        into view
                        select new ProductSelect()
                        {
                            Name = view.Key.product.Name,
                            Id = view.Key.product.Id,
                            BrandName = view.Key.brand.Name,
                            CateName = view.Key.cate.Name,
                            ListProductDetail = (from tetsa in queryDetail
                                                 where tetsa.IdProduct == view.Key.product.Id
                                                 && tetsa.Status == ProductDetailStatus.Stocking
                                                 select new ProductDetailView
                                                 {
                                                     IdProductDetail = tetsa.Id,
                                                     Name = tetsa.Name,
                                                     Price = tetsa.Price,
                                                     Quantity = tetsa.Amount,
                                                     Status = tetsa.Status,
                                                 }).ToList(),
                            LinkImg = view.Key.img.UrlImage,
                        };

            return new ResponseData<List<ProductSelect>>() { IsSuccess = true, Data = query.ToList() };
        }
    }
}
