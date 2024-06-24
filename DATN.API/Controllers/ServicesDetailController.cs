using DATN.Aplication.Services;
using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.DTOs.ServiceDetail;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesDetailController : ControllerBase
    {
        private readonly IServiceDetailManagementService _serviceDetailManagementService;

        public ServicesDetailController(IServiceDetailManagementService serviceDetailManagementService)
        {
            _serviceDetailManagementService = serviceDetailManagementService;
        }

        [HttpGet("getAllServicesDetail")]
        public async Task<IActionResult> GetAllServicesDetail()
        {
            var result = await _serviceDetailManagementService.GetAllService();

            if (result.IsSuccess == false) return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("getServiceDetailById/{id}")]
        public async Task<IActionResult> GetServiceDetailById(int id)
        {
            if (id == 0 || id == null) return BadRequest();

            var result = await _serviceDetailManagementService.GetServiceById(id);

            if (result.IsSuccess == false) return BadRequest(result);

            return Ok(result);
        }

        [HttpPost("createServiceDetail")]
        public async Task<IActionResult> CreateServiceDetail(CreateServiceDetailVM serviceDetail)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _serviceDetailManagementService.CreateNewService(serviceDetail);

            if (result.IsSuccess == false) return BadRequest(result);

            return Ok(result);
        }

        [HttpPut("updateServiceDetail/{id}")]
        public async Task<IActionResult> UpdateServiceDetail(int id, UpdateServiceDetailVM serviceDetail)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _serviceDetailManagementService.UpdateService(id, serviceDetail);

            if (result.IsSuccess == false) return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("removeServiceDetail/{id}")]
        public async Task<IActionResult> RemoveServiceDetail(int id)
        {
            if (id == 0 || id == null) return BadRequest();

            var result = await _serviceDetailManagementService.RemoveService(id);

            if (result.IsSuccess == false) return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("getServiceDetailByServiceId/{id}")]
        public async Task<IActionResult> GetServiceDetailByServiceId(int id)
        {
            if (id == 0 || id == null) return BadRequest();

            var result = await _serviceDetailManagementService.GetServicesByIdService(id);

            if (result.IsSuccess == false) return BadRequest(result);

            return Ok(result);
        }

        [HttpGet("getServiceName")]
        public async Task<IActionResult> GetServiceName()
        {
            var result = await _serviceDetailManagementService.GetServiceName();
            if(result != null) return Ok(result);

            return BadRequest();
        }
    }
}

