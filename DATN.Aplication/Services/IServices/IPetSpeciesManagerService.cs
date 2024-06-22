using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Guest;
using DATN.ViewModels.DTOs.PetSpecies;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services.IServices
{
	public interface IPetSpeciesManagerService
	{
		Task<ResponseData<string>> Create(PetSpeciesCreateUpdate request);
		Task<ResponseData<string>> Update(PetSpeciesCreateUpdate request);
		Task<ResponseData<List<PetSpeciesVM>>> GetAll();
		Task<ResponseData<PetSpeciesVM>> FindPetSpeciesByID(int id);
		Task<ResponseData<string>> SoftDelete(DeleteRequest<int> request);
	}
}
