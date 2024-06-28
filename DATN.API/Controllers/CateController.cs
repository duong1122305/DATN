using DATN.Aplication.Services;
using DATN.Aplication.Services.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Category;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CateController : Controller
    {
        ICategoryManagement _categoryManagement;
        public CateController(ICategoryManagement categoryManagement)
        {
            _categoryManagement = categoryManagement;
        }
        [HttpPost("Create-Category")]
        public Task<ResponseData<string>> CreateCategory(CategoryView categoryView)
        {
            return _categoryManagement.CreateCategory(categoryView);
        }
        [HttpPut("Update-Category")]

        public Task<ResponseData<string>> UpdateCategory(CategoryView categoryView)
        {
            return _categoryManagement.UpdateCategory(categoryView);
        }
        [HttpGet("Remove-Category")]
        public Task<ResponseData<string>> RemoveCategory(int id)
        {
            return _categoryManagement.RemoveCategory(id);
        }
        [HttpGet("List-Category")]
        public async Task<ResponseData<List<CategoryView>>> ListCategory()
        {
            return await _categoryManagement.ListCategory();
        }
        [HttpGet("Active-Category")]
        public Task<ResponseData<string>> ActiveCategory(int id)
        {
            return _categoryManagement.Active(id);
        }
    }
}
