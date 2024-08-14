using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Category;

namespace DATN.Aplication.Services
{
    public class CategoryManagementService : ICategoryManagementService
    {
        IUnitOfWork _unitOfWork;
        public CategoryManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<string>> CreateCategory(CategoryView categoryView)
        {

            try
            {
                var checkdup = from cate in await _unitOfWork.CategoryRepository.GetAllAsync()
                               where cate.Name == categoryView.Name.Trim().TrimStart().TrimEnd()
                               select cate;
                if (checkdup.Count() == 0)
                {
                    var category = new Category()
                    {
                        Name = categoryView.Name,
                        Description = categoryView.Description,
                        IsDeleted = false
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
                               where cate.Name == categoryView.Name.Trim().TrimStart().TrimEnd()
                               select cate;
                if (checkdup.Count() != 0)
                {
                    if (category.Id == checkdup.FirstOrDefault().Id)
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
                else
                {
                    category.Name = categoryView.Name;
                    category.Description = categoryView.Description;
                    await _unitOfWork.CategoryRepository.UpdateAsync(category);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công " };
                }

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
                var category = (from cate in await _unitOfWork.CategoryRepository.GetAllAsync()
                                where cate.Id == id
                                select cate).FirstOrDefault();
                if (category != null)
                {

                    category.IsDeleted = true;
                    await _unitOfWork.CategoryRepository.UpdateAsync(category);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Cập nhật trạng thái thành công" };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Không có danh mục này" };

            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Cập nhật trạng thái không thành công" };
            }
        }
        public async Task<ResponseData<string>> Active(int id)
        {
            try
            {
                var category = (from cate in await _unitOfWork.CategoryRepository.GetAllAsync()
                                where cate.Id == id
                                select cate).FirstOrDefault();
                if (category != null)
                {

                    category.IsDeleted = false;
                    await _unitOfWork.CategoryRepository.UpdateAsync(category);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Cập nhật trạng thái thành công" };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Không có danh mục này" };

            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Cập nhật trạng thái không thành công" };
            }
        }
        public async Task<ResponseData<List<CategoryView>>> ListCategory()
        {
            var list = from cate in await _unitOfWork.CategoryRepository.GetAllAsync()
                       select new CategoryView
                       {
                           Id = cate.Id,
                           Name = cate.Name,
                           Description = cate.Description,
                           IsDeleted = cate.IsDeleted
                       };
            if (list.Count() > 0)
                return new ResponseData<List<CategoryView>> { IsSuccess = true, Data = list.OrderByDescending(p => p.Id).ToList() };
            else
                return new ResponseData<List<CategoryView>> { IsSuccess = false, Data = new List<CategoryView>(), Error = "Chưa có dữ liệu" };
        }
    }
}

