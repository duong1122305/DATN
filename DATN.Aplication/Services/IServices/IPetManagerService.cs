﻿using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Pet;

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
        Task<ResponseData<Pet>> GetPetById(int id);
        Task<ResponseData<List<PetVM>>> GetListPetOfGuest(string id);
    }
}
