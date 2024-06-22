using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Pet;

namespace DATN.ADMIN.IServices
{
    public interface IPetServiceClient
    {
		Task<ResponseData<List<PetVM>>> GetPetByGuestId(Guid guestId);
		Task<ResponseData<List<PetVM>>> GetPetBySpeciesId(int id);
		Task<ResponseData<string>> CreatePet(PetCreateUpdate petVM);
		Task<ResponseData<string>> UpdatePet(PetCreateUpdate petVM);
		Task<ResponseData<string>> SoftDelete(DeleteRequest<int> request);
		Task<ResponseData<List<PetVM>>> GetAll();
	}
}
