using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Pet;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services.IServices
{
	public interface IPetManagerService
	{
		Task<ResponseData<List<PetVM>>> GetPetByGuestId(Guid guestId);
		Task<ResponseData<List<PetVM>>> GetPetBySpeciesId(int id);
		Task<ResponseData<string>> CreatePet(PetCreateUpdate petVM);
		Task<ResponseData<string>> UpdatePet(PetCreateUpdate petVM);
		Task<ResponseData<string>> SoftDelete(DeleteRequest<int> request);
		Task<ResponseData<List<PetVM>>> GetAll();
		Task<ResponseData<List<PetType>>> GetAllTypes();
	}
}
