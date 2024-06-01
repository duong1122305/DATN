using AutoMapper;
using DATN.Aplication.Services;
using DATN.Data.Entities;
using DATN.ViewModels.DTOs.Guest;
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
        }
    }
}
