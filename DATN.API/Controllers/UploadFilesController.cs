using DATN.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFilesController : ControllerBase
    {
        private readonly IUploadFileServices _services;

        public UploadFilesController(IUploadFileServices services)
        {
            _services = services;
        }

        [HttpPost("uploadAvatar/{idUser}")]
        public async Task<IActionResult> UploadAvatar(Guid idUser, IFormFile file)
        {
            var result = await _services.UploadAvatarAsync(idUser, file);
            if (result.IsSuccess) return Ok(result);

            return BadRequest(result);
        }
    }
}
