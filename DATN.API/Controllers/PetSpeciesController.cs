using DATN.Aplication.Services.IServices;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.PetSpecies;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class PetSpeciesController : ControllerBase
    {
        private readonly IPetSpeciesManagerService _service;

        public PetSpeciesController(IPetSpeciesManagerService service)
        {
            _service = service;
        }

        [HttpGet("get-all")]
        public async Task<ResponseData<List<PetSpeciesVM>>> GetALl()
        {
            return await _service.GetAll();
        }

        [HttpPost("create-species")]
        public async Task<ResponseData<string>> Create(PetSpeciesCreateUpdate resquest)
        {
            return await _service.Create(resquest);

        }
        [HttpPost("update-species")]
        public async Task<ResponseData<string>> Update(PetSpeciesCreateUpdate resquest)
        {
            return await _service.Update(resquest);
        }

        [HttpGet("get-by-id-species")]
        public async Task<ResponseData<PetSpeciesVM>> GetById(int id)
        {
            return await _service.FindPetSpeciesByID(id);
        }

        [HttpPost("delete-species")]
        public async Task<ResponseData<string>> Delete(DeleteRequest<int> resquest)
        {
            return await _service.SoftDelete(resquest);
        }
    }
}
