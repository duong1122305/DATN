using AutoMapper;
using DATN.Data.Entities;
using DATN.ViewModels.DTOs.Guest;
using DATN.ViewModels.DTOs.Pet;
using DATN.ViewModels.DTOs.PetSpecies;

namespace DATN.Aplication.Mapping
{
    public class MappingProfile : Profile
    {

        public MappingProfile()
        {
            CreateMap<GuestRegisterByGuestRequest, Guest>().ReverseMap();
            CreateMap<Guest, GuestViewModel>().ReverseMap();
            CreateMap<PetSpeciesCreateUpdate, PetSpecies>().ReverseMap();
            CreateMap<PetCreateUpdate, Pet>();
            CreateMap<Pet, PetVM>();
        }
    }
}
