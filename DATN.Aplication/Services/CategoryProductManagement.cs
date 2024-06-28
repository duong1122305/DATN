using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Category;
using DATN.ViewModels.DTOs.CategoryProduct;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
    public class CategoryProductManagement
    {
        IUnitOfWork _unitOfWork;
        public CategoryProductManagement(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<string>> CreateCategory(CategoryProductView categoryView)
        {

            try
            {
                var check = from cate in await _unitOfWork.CategoryProductRepository.GetAllAsync()
                            where cate.Name == categoryView.Name
                            select cate;
                if (check.Count() == 0)
                {
                    var category = new CategoryProduct()
                    {
                        Name = categoryView.Name,
                        IdCategory = categoryView.IdCategory,
                        IsDeleted = false,
                    };
                    await _unitOfWork.CategoryProductRepository.AddAsync(category);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Thêm thành công " };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Tên loại sản phẩm trùng với loại sản phẩm đã có" };
            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Thêm không thành công " };
            }
        }
        public async Task<ResponseData<string>> UpdateCategory(CategoryProductView categoryView)
        {

            try
            {
                var category = (from cate in await _unitOfWork.CategoryProductRepository.GetAllAsync()
                                where cate.Id == categoryView.Id
                                select cate).FirstOrDefault();
                var checkdup = from cate in await _unitOfWork.CategoryProductRepository.GetAllAsync()
                               where cate.Name == categoryView.Name
                               select cate;
                if (category.Id == checkdup.FirstOrDefault().Id)
                {
                    category.Name = categoryView.Name;
                    category.IdCategory = categoryView.IdCategory;
                    await _unitOfWork.CategoryProductRepository.UpdateAsync(category);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công " };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Tên loại sản phẩm trùng với loại sản phẩm đã có" };

            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Sửa không thành công " };
            }
        }
        public async Task<ResponseData<string>> RemoveCategory(int id)
        {

            try
            {
                var category = (from cate in await _unitOfWork.CategoryProductRepository.GetAllAsync()
                                where cate.Id == id
                                select cate).FirstOrDefault();
                if (category != null)
                {

                    category.IsDeleted = true;
                    await _unitOfWork.CategoryProductRepository.UpdateAsync(category);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Xóa thành công " };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Không có category này" };

            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Xóa không thành công " };
            }
        }
        public async Task<ResponseData<string>> Active(int id)
        {
            try
            {
                var category = (from cate in await _unitOfWork.CategoryProductRepository.GetAllAsync()
                                where cate.Id == id
                                select cate).FirstOrDefault();
                if (category != null)
                {

                    category.IsDeleted = false;
                    await _unitOfWork.CategoryProductRepository.UpdateAsync(category);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Active thành công " };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Không có category này" };

            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Active không thành công " };
            }
        }
        public async Task<ResponseData<List<CategoryProductView>>> ListCategory()
        {
            var list = from cate in await _unitOfWork.CategoryProductRepository.GetAllAsync()
                       select new CategoryProductView
                       {
                           Id = cate.Id,
                           Name = cate.Name,
                           IdCategory = cate.IdCategory,
                           IsDeleted=cate.IsDeleted,
                       };
            if (list.Count() > 0)
                return new ResponseData<List<CategoryProductView>> { IsSuccess = true, Data = list.ToList() };
            else
                return new ResponseData<List<CategoryProductView>> { IsSuccess = false, Data = new List<CategoryProductView>(), Error = "Chưa có dữ liệu" };
        }
    }
}
