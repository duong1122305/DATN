﻿using DATN.Aplication.Services.IServices;
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
        public async Task<ResponseData<string>> CreateProduct(CreateProductView productView)
        {
            var check = (from product in await _unitOfWork.ProductRepository.GetAllAsync()
                         where product.Name == productView.Name
                         select product).FirstOrDefault();
            try
            {
                if (productView == null)
                {
                    var product = new Product()
                    {
                        IdBrand = productView.IdBrand,
                        Name = productView.Name,
                        Description = productView.Description,
                        IdCategoryProduct = productView.IdCategoryProduct,
                        Status = true,
                    };
                    await _unitOfWork.ProductRepository.AddAsync(product);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Thêm sản phẩm thành công" };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Tên sản phẩm bị trùng" };
            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Lỗi liên quan đến server vui lòng liên hệ dev để fix" };
            }
        }
        public async Task<ResponseData<string>> UpdateProduct(CreateProductView productView)
        {
            try
            {
                var product = (from cate in await _unitOfWork.ProductRepository.GetAllAsync()
                               where cate.Id == productView.Id
                               select cate).FirstOrDefault();
                var checkdup = from cate in await _unitOfWork.CategoryRepository.GetAllAsync()
                               where cate.Name == productView.Name
                               select cate;
                if (checkdup.Count() > 0)
                {
                    if (product.Id == checkdup.FirstOrDefault().Id)
                    {
                        product.Name = productView.Name;
                        product.Description = productView.Description;
                        product.IdBrand = productView.IdBrand;
                        product.IdCategoryProduct = productView.IdCategoryProduct;
                        await _unitOfWork.ProductRepository.UpdateAsync(product);
                        await _unitOfWork.SaveChangeAsync();
                        return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công " };
                    }
                    else
                        return new ResponseData<string> { IsSuccess = false, Error = "Tên loại sản phẩm trùng với loại sản phẩm đã có" };
                }
                else
                {
                    product.Name = productView.Name;
                    product.Description = productView.Description;
                    product.IdBrand = productView.IdBrand;
                    product.IdCategoryProduct = productView.IdCategoryProduct;
                    await _unitOfWork.ProductRepository.UpdateAsync(product);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công " };
                }

            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Lỗi liên quan đến server vui lòng liên hệ dev để fix" };
            }
        }
        public async Task<ResponseData<string>> RemoveProduct(int id)
        {
            try
            {
                var product = (from cate in await _unitOfWork.ProductRepository.GetAllAsync()
                               where cate.Id == id
                               select cate).FirstOrDefault();
                if (product != null)
                {
                    product.Status = false;
                    await _unitOfWork.ProductRepository.UpdateAsync(product);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Xóa thành công " };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Không tìm thấy sản phẩm cần xóa" };

            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Lỗi liên quan đến server vui lòng liên hệ dev để fix" };
            }
        }
        public async Task<ResponseData<string>> ActiveProduct(int id)
        {
            try
            {
                var product = (from cate in await _unitOfWork.ProductRepository.GetAllAsync()
                               where cate.Id == id
                               select cate).FirstOrDefault();
                if (product != null)
                {
                    product.Status = true;
                    await _unitOfWork.ProductRepository.UpdateAsync(product);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Active thành công " };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Không tìm thấy sản phẩm cần xóa" };

            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Lỗi liên quan đến server vui lòng liên hệ dev để fix" };
            }
        }
        public async Task<ResponseData<List<ProductDetaiView>>> ListProduct()
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
