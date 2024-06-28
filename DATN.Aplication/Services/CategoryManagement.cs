using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
    public class CategoryManagement
    {
        IUnitOfWork _unitOfWork;
        public CategoryManagement(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<string>> CreateCategory(CategoryView categoryView)
        {

            try
            {
                var checkdup = from cate in await _unitOfWork.CategoryRepository.GetAllAsync()
                               where cate.Name == categoryView.Name
                               select cate;
                if (checkdup.Count() == 0)
                {
                    var category = new Category()
                    {
                        Name = categoryView.Name,
                        Description = categoryView.Description,
                    };
                    await _unitOfWork.CategoryRepository.AddAsync(category);
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
        public async Task<ResponseData<string>> UpdateCategory(CategoryView categoryView)
        {

            try
            {
                var category = (from cate in await _unitOfWork.CategoryRepository.GetAllAsync()
                                where cate.Id == categoryView.Id
                                select cate).FirstOrDefault();
                var checkdup = from cate in await _unitOfWork.CategoryRepository.GetAllAsync()
                               where cate.Name == categoryView.Name
                               select cate;
                if (category.Id==checkdup.FirstOrDefault().Id)
                {
                    category.Name = categoryView.Name;
                    category.Description = categoryView.Description;
                    await _unitOfWork.CategoryRepository.UpdateAsync(category);
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
    }
}
