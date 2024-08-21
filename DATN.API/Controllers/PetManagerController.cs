using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Pet;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [EnableCors("AllowSpecificOrigin")]
    public class PetManagerController : ControllerBase
    {
        private readonly IPetManagerService _petManagerService;

        public PetManagerController(IPetManagerService petManagerService)
        {
            _petManagerService = petManagerService;
        }
        [HttpGet("get-pet-by-species")]
        public async Task<ResponseData<List<PetVM>>> GetPetBySpeciesId(int id)
        {

            return await _petManagerService.GetPetBySpeciesId(id);
        }
        [HttpGet("get-pet-by-guest")]
        public async Task<ResponseData<List<PetVM>>> GetPetByGuestId(Guid id)
        {
            return await _petManagerService.GetPetByGuestId(id);
        }
        [HttpGet("get-all-pet")]
        public async Task<ResponseData<List<PetVM>>> GetAllPet()
        {
            return await _petManagerService.GetAll();
        }
        [HttpPost("create-pet")]
        public async Task<ResponseData<string>> CreatePet(PetCreateUpdate petCU)
        {
            return await _petManagerService.CreatePet(petCU);
        }
        [HttpPost("soft-delete-pet")]
        public async Task<ResponseData<string>> SoftDelete(DeleteRequest<int> request)
        {
            return await _petManagerService.SoftDelete(request);
        }
        [HttpPost("update-pet")]
        public async Task<ResponseData<string>> UpdatePet(PetCreateUpdate petCU)
        {
            return await _petManagerService.UpdatePet(petCU);
        }

        [HttpGet("get-types")]
        public async Task<ResponseData<List<PetType>>> GetAllType()
        {
            return await _petManagerService.GetAllTypes();
        }

        [HttpGet("GetById")]
        public async Task<ResponseData<Pet>> GetById(int id)
        {
            return await _petManagerService.GetPetById(id);
        }
    }
}
