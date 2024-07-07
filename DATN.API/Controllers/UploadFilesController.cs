using CloudinaryDotNet.Actions;
using CloudinaryDotNet;
using DATN.Data.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using DATN.Data.EF;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UploadFilesController : ControllerBase
    {
        private readonly IOptions<CloundinarySettings> _cloundinarySettings;
        private DATNDbContext _context;

        public UploadFilesController(IOptions<CloundinarySettings> cloundinarySettings, DATNDbContext context)
        {
            _cloundinarySettings = cloundinarySettings;
            _context = context;
        }

        [HttpPost("{userId}/files")]
        public async Task<IActionResult> UploadFiles(int procId, List<IFormFile> files)
        {
            if (procId == null) return BadRequest();

            var account = new Account(
                _cloundinarySettings.Value.CloundName,
                _cloundinarySettings.Value.ApiKey,
                _cloundinarySettings.Value.ApiSecret
                );
            var cloundinary = new Cloudinary(account);

            var uploadResults = new List<Dictionary<string, string>>();

            foreach (var i in files)
            {
                var uploadParams = new ImageUploadParams()
                {
                    File = new FileDescription(i.FileName, i.OpenReadStream()),
                    Folder = $"uploads/users/{procId}"
                };

                var uploadResult = await cloundinary.UploadAsync(uploadParams);

                uploadResults.Add(new Dictionary<string, string>
                {
                    {"public_id", uploadResult.PublicId },
                    {"url", uploadResult.SecureUrl.AbsoluteUri }
                });

                await _context.ImageProducts.AddAsync(new ImageProduct { IdProduct = procId, UrlImage = uploadResult.SecureUrl.AbsoluteUri, IsDefault = false });
            }
            await _context.SaveChangesAsync();
            return Ok(uploadResults);
        }
    }
}
