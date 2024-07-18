using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Product;
using DATN.ViewModels.DTOs.ProductDetail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
                if (productView != null)
                {
                    var product = new ProductDetail()
                    {
                        Name = productView.Name,
                        Amount = productView.Amount,
                        IdProduct = productView.IdProduct,
                        IsDeleted = false,
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
                               where cate.Name == productView.Name.Trim().TrimStart().TrimEnd()
                               select cate;
                if (checkdup.Count() > 0)
                {
                    if (product.Id == checkdup.FirstOrDefault().Id)
                    {
                        product.Name = productView.Name;
                        product.Price = productView.Price;
                        product.Amount = productView.Amount;
                        await _unitOfWork.ProductDetailRepository.UpdateAsync(product);
                        await _unitOfWork.SaveChangeAsync();
                        return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công " };
                    }
                    else
                        return new ResponseData<string> { IsSuccess = false, Error = "Tên loại sản phẩm trùng với loại sản phẩm đã có" };
                }
                else
                {
                    product.Name = productView.Name;
                    product.Price = productView.Price;
                    product.Amount = productView.Amount;
                    await _unitOfWork.ProductDetailRepository.UpdateAsync(product);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công " };
                }

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
                    product.IsDeleted = true;
                    await _unitOfWork.ProductDetailRepository.UpdateAsync(product);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Xóa thành công " };
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
                    product.IsDeleted = false;
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
                        select new ProductDetaiView
                        {
                            Id = productDetail.Id,
                            Amount = productDetail.Amount,
                            IsDeleted = productDetail.IsDeleted,
                            Name = productDetail.Name,
                            ProductId = productDetail.IdProduct,
                            Product = product.Name,
                            Price = productDetail.Price,
                        };
            if (query.Count()>0)
            {
                return new ResponseData<List<ProductDetaiView>> { IsSuccess=true,Data=query.ToList() };
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
                            IsDeleted = productde.IsDeleted,
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
