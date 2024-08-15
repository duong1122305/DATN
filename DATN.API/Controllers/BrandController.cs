using DATN.Aplication.Services.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Brand;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    [EnableCors("AllowSpecificOrigin")]
    public class BrandController : Controller
    {
        IBrandManagementService _brandManagementService;
        public BrandController(IBrandManagementService brandManagementService)
        {
            _brandManagementService = brandManagementService;
        }
        [HttpPost("Create-Brand")]
        public async Task<ResponseData<string>> CreateBrand(BrandView brandView)
        {
            return await _brandManagementService.CreateBrand(brandView);
        }

        [HttpPatch("Update-Brand")]
        public async Task<ResponseData<string>> UpdateBrand(BrandView brandView)
        {
            return await _brandManagementService.UpdateCategory(brandView);
        }

        [HttpGet("Remove-Brand")]
        public async Task<ResponseData<string>> RemoveBrand(int id)
        {
            return await _brandManagementService.RemoveCategory(id);
        }

        [HttpGet("Active-Brand")]
        public async Task<ResponseData<string>> Active(int id)
        {
            return await _brandManagementService.Active(id);
        }

        [HttpGet("List-Brand")]
        public async Task<ResponseData<List<BrandView>>> ListBrand()
        {
            return await _brandManagementService.ListBrand();
        }
    }
}
