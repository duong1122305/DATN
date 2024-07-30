﻿using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.ProductDetail;
using DATN.ViewModels.Enum;

namespace DATN.Aplication.Services
{
    public class ProductDetaiManagementService : IProductDetaiManagementService
    {
        IUnitOfWork _unitOfWork;
        public ProductDetaiManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<string>> CreateProductDetail(CreateProductDetaiView productView)
        {
            var check = (from product in await _unitOfWork.ProductDetailRepository.GetAllAsync()
                         where product.Name == productView.Name.Trim().TrimStart().TrimEnd()
                         select product).FirstOrDefault();
            try
            {
                if (check == null)
                {
                    var product = new ProductDetail()
                    {
                        Name = productView.Name,
                        Amount = productView.Amount,
                        IdProduct = productView.IdProduct,
                        Status = ProductDetailStatus.Stocking,
                        Price = productView.Price,
                    };
                    await _unitOfWork.ProductDetailRepository.AddAsync(product);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Thêm sản phẩm thành công" };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Tên sản phẩm bị trùng" };
            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Lỗi hệ thống! Vui lòng liên hệ nhà phát triển" };
            }
        }
        public async Task<ResponseData<string>> UpdateProductDetail(CreateProductDetaiView productView)
        {
            try
            {
                var product = (from cate in await _unitOfWork.ProductDetailRepository.GetAllAsync()
                               where cate.Id == productView.Id
                               select cate).FirstOrDefault();
                var checkdup = from cate in await _unitOfWork.ProductDetailRepository.GetAllAsync()
                               where cate.Name == productView.Name.Trim() && cate.Id != productView.Id && cate.IdProduct == productView.IdProduct
                               select cate;

                if (checkdup == null || checkdup.FirstOrDefault() == null)
                {
                    product.Name = productView.Name;
                    product.Price = productView.Price;
                    product.Amount = productView.Amount;
                    if (productView.Amount==0)
                    {
                        product.Status = ProductDetailStatus.OutOfStock;
                    }
                    else
                    {
						product.Status = ProductDetailStatus.Stocking;
					}
                    await _unitOfWork.ProductDetailRepository.UpdateAsync(product);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công " };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Tên loại sản phẩm trùng với loại sản phẩm đã có" };



            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Lỗi hệ thống! Vui lòng liên hệ nhà phát triển" };
            }
        }
        public async Task<ResponseData<string>> RemoveProductDetail(int id)
        {
            try
            {
                var product = (from cate in await _unitOfWork.ProductDetailRepository.GetAllAsync()
                               where cate.Id == id
                               select cate).FirstOrDefault();
                if (product != null)
                {
                    product.Status = ProductDetailStatus.Deleted;
                    await _unitOfWork.ProductDetailRepository.UpdateAsync(product);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Thay đổi thành công " };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Không tìm thấy sản phẩm cần xóa" };

            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Lỗi hệ thống! Vui lòng liên hệ nhà phát triển" };
            }
        }
        public async Task<ResponseData<string>> ActiveProductDetail(int id)
        {
            try
            {
                var product = (from cate in await _unitOfWork.ProductDetailRepository.GetAllAsync()
                               where cate.Id == id
                               select cate).FirstOrDefault();
                if (product != null)
                {
                    product.Status = ProductDetailStatus.Stocking;
                    if (product.Amount==0)
                    {
						product.Status = ProductDetailStatus.OutOfStock;
					}
                    await _unitOfWork.ProductDetailRepository.UpdateAsync(product);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Active thành công " };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Không tìm thấy sản phẩm cần xóa" };

            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Lỗi hệ thống! Vui lòng liên hệ nhà phát triển" };
            }
        }
        public async Task<ResponseData<List<ProductDetaiView>>> ListProductDetailForProduct(int id)
        {
            var query = from product in await _unitOfWork.ProductRepository.GetAllAsync()
                        join productDetail in await _unitOfWork.ProductDetailRepository.GetAllAsync()
                        on product.Id equals productDetail.IdProduct
                        where productDetail.IdProduct == id
                        orderby productDetail.Price
                        select new ProductDetaiView
                        {
                            Id = productDetail.Id,
                            Amount = productDetail.Amount,
                            Status = productDetail.Status,
                            Name = productDetail.Name,
                            ProductId = productDetail.IdProduct,
                            Product = product.Name,
                            Price = productDetail.Price,
                        };
            if (query.Count() > 0)
            {
                return new ResponseData<List<ProductDetaiView>> { IsSuccess = true, Data = query.ToList() };
            }
            return new ResponseData<List<ProductDetaiView>> { IsSuccess = false, Error = "Chưa có sản phẩm chi tiết của sản phẩm" };
        }
        public async Task<ResponseData<List<ProductDetaiView>>> ListProductDetail()
        {
            var query = from productde in await _unitOfWork.ProductDetailRepository.GetAllAsync()
                        join product in await _unitOfWork.ProductRepository.GetAllAsync()
                        on productde.IdProduct equals product.Id
                        select new ProductDetaiView()
                        {
                            Id = product.Id,
                            Amount = productde.Amount,
                            Status = productde.Status,
                            Name = productde.Name,
                            Price = productde.Price,
                            Product = product.Name,
                            ProductId = product.Id,
                        };
            if (query.Count() > 0)
                return new ResponseData<List<ProductDetaiView>> { IsSuccess = true, Data = query.ToList() };
            else
                return new ResponseData<List<ProductDetaiView>> { IsSuccess = false, Error = "Chưa có dữ liệu", Data = new List<ProductDetaiView>() };
        }
    }
}
