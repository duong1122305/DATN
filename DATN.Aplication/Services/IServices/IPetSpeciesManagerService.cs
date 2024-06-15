using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services.IServices
{
	public interface IPetSpeciesManagerService
	{
		Task<ResponseData<string>> Create(PetSpecies request);
		Task<ResponseData<string>> Update(PetSpecies request);
		Task<ResponseData<List<PetSpecies>>> GetAll();
		Task<ResponseData<PetSpecies>> FindGuestByID(int id);
		Task<ResponseData<string>> SoftDelete(DeleteRequest<Guid> request);
	}
}
