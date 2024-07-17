using DATN.Aplication.Services.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Product;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : Controller
    {
        IProductManagementService _productManagementServicel;
        public ProductController(IProductManagementService productManagementService)
        {
            _productManagementServicel = productManagementService;
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
        public async Task<ResponseData<string>> CreateProductDetails(CreateProductView productView)
        {
            return await _productManagementServicel.CreateProduct(productView);
        }
        [HttpPut("Update-Product-Details")]
        public async Task<ResponseData<string>> UpdateProductDetails(CreateProductView productView)
        {
            return await _productManagementServicel.UpdateProduct(productView);
        }
        [HttpGet("Remove-Product-Details")]
        public async Task<ResponseData<string>> RemoveProductDetails(int id)
        {
            return await _productManagementServicel.RemoveProduct(id);

        }
        [HttpGet("Active-Product-Details")]
        public async Task<ResponseData<string>> ActiveProductDetails(int id)
        {
            return await _productManagementServicel.ActiveProduct(id);

        }
        [HttpGet("List-Product-Details")]
        public async Task<ResponseData<List<ProductView>>> ListProductDetails()
        {
            return await _productManagementServicel.ListProduct();
        }
    }
}
