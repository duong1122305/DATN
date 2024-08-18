using DATN.Aplication.Services.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Category;
using DATN.ViewModels.DTOs.CategoryProduct;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class CateController : Controller
    {
        ICategoryManagementService _categoryManagement;
        ICategoryProductManagementService _categoryProductManagement;
        public CateController(ICategoryManagementService categoryManagement, ICategoryProductManagementService categoryProductManagement)
        {
            _categoryManagement = categoryManagement;
            _categoryProductManagement = categoryProductManagement;
        }
        [Authorize(Roles = "Admin")]

        [HttpPost("Create-Category")]
        public async Task<ResponseData<string>> CreateCategory(CategoryView categoryView)
        {
            return await _categoryManagement.CreateCategory(categoryView);
        }
        [Authorize(Roles = "Admin")]

        [HttpPatch("Update-Category")]

        public async Task<ResponseData<string>> UpdateCategory(CategoryView categoryView)
        {
            return await _categoryManagement.UpdateCategory(categoryView);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("Remove-Category")]
        public async Task<ResponseData<string>> RemoveCategory(int id)
        {
            return await _categoryManagement.RemoveCategory(id);
        }
        [HttpGet("List-Category")]
        public async Task<ResponseData<List<CategoryView>>> ListCategory()
        {
            return await _categoryManagement.ListCategory();
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("Active-Category")]
        public async Task<ResponseData<string>> ActiveCategory(int id)
        {
            return await _categoryManagement.Active(id);
        }
        [Authorize(Roles = "Admin")]

        [HttpPost("Create-Category-Product")]
        public Task<ResponseData<string>> CreateCategoryProduct(CreateCategoryProductView categoryView)
        {
            return _categoryProductManagement.CreateCategory(categoryView);
        }
        [Authorize(Roles = "Admin")]

        [HttpPatch("Update-Category-Product")]

        public Task<ResponseData<string>> UpdateCategoryProduct(CreateCategoryProductView categoryView)
        {
            return _categoryProductManagement.UpdateCategory(categoryView);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("Remove-Category-Product")]
        public Task<ResponseData<string>> RemoveCategoryProduct(int id)
        {
            return _categoryProductManagement.RemoveCategory(id);
        }
        [HttpGet("List-Category-Product")]
        public async Task<ResponseData<List<CategoryProductView>>> ListCategoryProduct()
        {
            return await _categoryProductManagement.ListCategory();
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("Active-Category-Product")]
        public Task<ResponseData<string>> ActiveCategoryProduct(int id)
        {
            return _categoryProductManagement.Active(id);
        }
    }
}
