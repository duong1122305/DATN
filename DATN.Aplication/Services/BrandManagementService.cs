using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Brand;
using DATN.ViewModels.DTOs.Category;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
    public class BrandManagementService : IBrandManagementService
    {
        IUnitOfWork _unitOfWork;
        public BrandManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<string>> CreateBrand(BrandView brandView)
        {
            try
            {
                var checkdup = from brand in await _unitOfWork.BrandRepository.GetAllAsync()
                               where brand.Name == brandView.Name
                               select brand;
                if (checkdup.Count() == 0)
                {
                    var brand = new Brand()
                    {
                        Name = brandView.Name,
                        Description = brandView.Description,
                        Status = true,
                    };
                    await _unitOfWork.BrandRepository.AddAsync(brand);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Thêm thành công " };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Tên thương hiệu trùng với thương hiệu đã có đã có" };
            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Thêm không thành công " };
            }
        }
        public async Task<ResponseData<string>> UpdateCategory(BrandView brandView)
        {
            try
            {
                var brand = (from cate in await _unitOfWork.BrandRepository.GetAllAsync()
                             where cate.Id == brandView.Id
                             select cate).FirstOrDefault();
                var checkdup = from cate in await _unitOfWork.BrandRepository.GetAllAsync()
                               where cate.Name == brandView.Name
                               select cate;
                if (checkdup.Count() == 0)
                {
                    brand.Name = brandView.Name;
                    brand.Description = brandView.Description;
                    await _unitOfWork.BrandRepository.UpdateAsync(brand);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công " };
                }
                else
                {
                    if (brand.Id == checkdup.FirstOrDefault().Id)
                    {
                        brand.Name = brandView.Name;
                        brand.Description = brandView.Description;
                        await _unitOfWork.BrandRepository.UpdateAsync(brand);
                        await _unitOfWork.SaveChangeAsync();
                        return new ResponseData<string> { IsSuccess = true, Data = "Sửa thành công " };
                    }
                    else
                        return new ResponseData<string> { IsSuccess = false, Error = "Tên loại sản phẩm trùng với loại sản phẩm đã có" };
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
                var brand = (from cate in await _unitOfWork.BrandRepository.GetAllAsync()
                             where cate.Id == id
                             select cate).FirstOrDefault();
                if (brand != null)
                {

                    brand.Status = false;
                    await _unitOfWork.BrandRepository.UpdateAsync(brand);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Xóa thành công " };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Không có danh mục này" };

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
                var brand = (from cate in await _unitOfWork.BrandRepository.GetAllAsync()
                             where cate.Id == id
                             select cate).FirstOrDefault();
                if (brand != null)
                {

                    brand.Status = true;
                    await _unitOfWork.BrandRepository.UpdateAsync(brand);
                    await _unitOfWork.SaveChangeAsync();
                    return new ResponseData<string> { IsSuccess = true, Data = "Kích hoạt thành công " };
                }
                else
                    return new ResponseData<string> { IsSuccess = false, Error = "Không có danh mục này" };

            }
            catch (Exception)
            {
                return new ResponseData<string> { IsSuccess = false, Error = "Kích hoạt không thành công " };
            }
        }
        public async Task<ResponseData<List<BrandView>>> ListBrand()
        {
            var list = from cate in await _unitOfWork.BrandRepository.GetAllAsync()
                       select new BrandView
                       {
                           Id = cate.Id,
                           Name = cate.Name,
                           Description = cate.Description,
                           Status = cate.Status
                       };
            if (list.Count() > 0)
                return new ResponseData<List<BrandView>> { IsSuccess = true, Data = list.ToList() };
            else
                return new ResponseData<List<BrandView>> { IsSuccess = false, Data = new List<BrandView>(), Error = "Chưa có dữ liệu!" };
        }
    }
}
