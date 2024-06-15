using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
	public class PetSpeciesManagerService : IPetSpeciesManagerService
	{
		private readonly IUnitOfWork _unitOfWork;

		public PetSpeciesManagerService(IUnitOfWork unitOfWork)
		{
			_unitOfWork = unitOfWork;
		}
		public async Task<ResponseData<string>> Create(PetSpecies request)
		{
			try
			{
				var data = await _unitOfWork.PetSpeciesRepository.AddAsync(request);
				return new ResponseData<string>("Thêm thành công");

			}
			catch (Exception)
			{
				return new ResponseData<string>()
				{
					IsSuccess = false,
					Error = "Có lỗi khi thêm dữ liệu"
				};
			}
		}

		public async Task<ResponseData<PetSpecies>> FindGuestByID(int id)
		{
			try
			{
				var data = await _unitOfWork.PetSpeciesRepository.GetAsync(id);
				return new ResponseData<PetSpecies>(data);

			}
			catch (Exception)
			{
				return new ResponseData<PetSpecies>()
				{
					IsSuccess = false,
					Error = "Có lỗi khi lấy dữ liệu"
				};
			}
		}

		public async Task<ResponseData<List<PetSpecies>>> GetAll()
		{
			try
			{
				var data = await _unitOfWork.PetSpeciesRepository.GetAllAsync();
				return new ResponseData<List<PetSpecies>>(data.ToList());

			}
			catch (Exception)
			{
				return new ResponseData<List<PetSpecies>>()
				{
					IsSuccess = false,
					Error = "Có lỗi khi lấy dữ liệu"
				};
			}
		}

		public Task<ResponseData<string>> SoftDelete(DeleteRequest<Guid> request)
		{
			throw new NotImplementedException();
		}

		public async Task<ResponseData<string>> Update(PetSpecies request)
		{

			try
			{
				var data = await _unitOfWork.PetSpeciesRepository.UpdateAsync(request);
				return new ResponseData<string>("Cập nhật thành công");

			}
			catch (Exception)
			{
				return new ResponseData<string>()
				{
					IsSuccess = false,
					Error = "Có lỗi khi cập nhật dữ liệu"
				};
			}
		}
	}
}
