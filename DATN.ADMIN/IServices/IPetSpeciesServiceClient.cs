using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.PetSpecies;

namespace DATN.ADMIN.IServices
{
	public interface IPetSpeciesServiceClient
	{
		Task<ResponseData<string>> Create(PetSpeciesCreateUpdate request);
		Task<ResponseData<string>> Update(PetSpeciesCreateUpdate request);
		Task<ResponseData<List<PetSpecies>>> GetAll();
		Task<ResponseData<PetSpecies>> FindPetSpeciesByID(int id);
		Task<ResponseData<string>> SoftDelete(DeleteRequest<int> request);
	}
}
