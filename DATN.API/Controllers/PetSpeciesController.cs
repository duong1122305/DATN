using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Guest;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace DATN.API.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class PetSpeciesController : ControllerBase
	{
		private readonly IPetSpeciesManagerService _service;

		public PetSpeciesController(IPetSpeciesManagerService service)
        {
			_service = service;
		}
        [HttpPost("get-all")]
		public async Task<ResponseData<List<PetSpecies>>> GetALl()
		{
			return await _service.GetAll();
		}
	}
}
