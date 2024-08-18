using DATN.Aplication.Services.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Product;
using DATN.ViewModels.DTOs.ProductDetail;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    // cấp quyền đê không bị chặn cors khi chạy local
    [EnableCors("AllowSpecificOrigin")]
    public class ProductController : Controller
    {
        IProductManagementService _productManagementServicel;
        IProductDetaiManagementService _productDetaiManagementServicel;
        public ProductController(IProductManagementService productManagementService, IProductDetaiManagementService productDetaiManagementService)
        {
            _productManagementServicel = productManagementService;
            _productDetaiManagementServicel = productDetaiManagementService;
        }
        [Authorize(Roles = "Admin")]

        [HttpPost("Create-Product")]
        public async Task<ResponseData<string>> CreateProduct(CreateProductView productView)
        {
            return await _productManagementServicel.CreateProduct(productView);
        }
        [Authorize(Roles = "Admin")]

        [HttpPatch("Update-Product")]
        public async Task<ResponseData<string>> UpdateProduct(CreateProductView productView)
        {
            return await _productManagementServicel.UpdateProduct(productView);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("Remove-Product")]
        public async Task<ResponseData<string>> RemoveProduct(int id)
        {
            return await _productManagementServicel.RemoveProduct(id);

        }
        [Authorize(Roles = "Admin")]

        [HttpGet("Active-Product")]
        public async Task<ResponseData<string>> ActiveProduct(int id)
        {
            return await _productManagementServicel.ActiveProduct(id);

        }
        [HttpGet("List-Product")]
        public async Task<ResponseData<List<ProductView>>> ListProduct()
        {
            return await _productManagementServicel.ListProduct();
        }
        [Authorize(Roles = "Admin")]

        //product details
        [HttpPost("Create-Product-Details")]
        public async Task<ResponseData<string>> CreateProductDetails(CreateProductDetaiView productView)
        {
            return await _productDetaiManagementServicel.CreateProductDetail(productView);
        }
        [Authorize(Roles = "Admin")]

        [HttpPatch("Update-Product-Details")]
        public async Task<ResponseData<string>> UpdateProductDetails(CreateProductDetaiView productView)
        {
            return await _productDetaiManagementServicel.UpdateProductDetail(productView);
        }
        [Authorize(Roles = "Admin")]

        [HttpGet("Remove-Product-Details")]
        public async Task<ResponseData<string>> RemoveProductDetails(int id)
        {
            return await _productDetaiManagementServicel.RemoveProductDetail(id);

        }
        [Authorize(Roles = "Admin")]

        [HttpGet("Active-Product-Details")]
        public async Task<ResponseData<string>> ActiveProductDetails(int id)
        {
            return await _productDetaiManagementServicel.ActiveProductDetail(id);

        }
        [HttpGet("List-Product-Details")]
        public async Task<ResponseData<List<ProductDetaiView>>> ListProductDetails()
        {
            return await _productDetaiManagementServicel.ListProductDetail();
        }
        [HttpGet("list-product-details-by-id")]
        public async Task<ResponseData<List<ProductDetaiView>>> ListProductDetailsByID(int id)
        {
            return await _productDetaiManagementServicel.ListProductDetailForProduct(id);
        }
    }
}
