using DATN.Aplication.Services.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Product;
using DATN.ViewModels.DTOs.ProductDetail;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        IProductManagementService _productManagementServicel;
        IProductDetaiManagementService _productDetaiManagementServicel;
        public ProductController(IProductManagementService productManagementService, IProductDetaiManagementService productDetaiManagementService)
        {
            _productManagementServicel = productManagementService;
            _productDetaiManagementServicel = productDetaiManagementService;
        }
        [HttpPost("Create-Product")]
        public async Task<ResponseData<string>> CreateProduct(CreateProductView productView)
        {
            return await _productManagementServicel.CreateProduct(productView);
        }
        [HttpPut("Update-Product")]
        public async Task<ResponseData<string>> UpdateProduct(CreateProductView productView)
        {
            return await _productManagementServicel.UpdateProduct(productView);
        }
        [HttpGet("Remove-Product")]
        public async Task<ResponseData<string>> RemoveProduct(int id)
        {
            return await _productManagementServicel.RemoveProduct(id);

        }
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

        //product details
        [HttpPost("Create-Product-Details")]
        public async Task<ResponseData<string>> CreateProductDetails(CreateProductDetaiView productView)
        {
            return await _productDetaiManagementServicel.CreateProductDetail(productView);
        }
        [HttpPut("Update-Product-Details")]
        public async Task<ResponseData<string>> UpdateProductDetails(CreateProductDetaiView productView)
        {
            return await _productDetaiManagementServicel.UpdateProductDetail(productView);
        }
        [HttpGet("Remove-Product-Details")]
        public async Task<ResponseData<string>> RemoveProductDetails(int id)
        {
            return await _productDetaiManagementServicel.RemoveProductDetail(id);

        }
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
    }
}
