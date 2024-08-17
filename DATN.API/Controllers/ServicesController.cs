using DATN.Aplication.Services.IServices;
using DATN.ViewModels.DTOs.ServiceManager;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    [Authorize(Roles = "Admin")]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceManagementService _serviceManager;

        public ServicesController(IServiceManagementService serviceManager)
        {
            _serviceManager = serviceManager;
        }

        [HttpGet("getAllService")]
        public async Task<IActionResult> GetAllService()
        {
            var result = await _serviceManager.GetAllService();

            if (result.Data == null && result.IsSuccess == false) return NotFound(result);

            return Ok(result);
        }

        [HttpGet("getServiceById/{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            if (id == 0 || id == null) return BadRequest("Không tìm thấy sản phẩm");

            var result = await _serviceManager.GetServiceById(id);

            if (result.Data == null && result.IsSuccess == false) return NotFound(result);

            return Ok(result);
        }

        [HttpPost("createService")]
        public async Task<IActionResult> CreateService(CreateServiceVM service)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _serviceManager.CreateNewService(service);

            if (result.IsSuccess == false) return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("updateService/{id}")]
        public async Task<IActionResult> UpdateService(int id, UpdateServiceVM service)
        {
            if (!ModelState.IsValid) return BadRequest(ModelState);

            var result = await _serviceManager.UpdateService(id, service);

            if (result.IsSuccess == false) return BadRequest(result);

            return Ok(result);
        }

        [HttpPatch("removeService/{id}")]
        public async Task<IActionResult> RemoveService(int id)
        {
            if (id == 0 || id == null) return BadRequest();

            var result = await _serviceManager.RemoveService(id);

            if (result.IsSuccess == false) return BadRequest(result);

            return Ok(result);
        }
    }
}
