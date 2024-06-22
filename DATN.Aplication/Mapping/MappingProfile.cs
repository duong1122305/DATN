using AutoMapper;
using DATN.Aplication.Repository;
using DATN.Aplication.Services;
using DATN.Data.Entities;
using DATN.ViewModels.DTOs.Guest;
using DATN.ViewModels.DTOs.Pet;
using DATN.ViewModels.DTOs.PetSpecies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Mapping
{
    public class MappingProfile:Profile
    {
       
        public MappingProfile()
        {
            CreateMap<GuestRegisterNoUserRequest, Guest>().ReverseMap();
            CreateMap<Guest, GuestViewModel>().ReverseMap();
            CreateMap<PetSpeciesCreateUpdate, PetSpecies>().ReverseMap();
            CreateMap<PetCreateUpdate, Pet>();
            CreateMap<Pet, PetVM>();
        }
    }
}
