using AutoMapper;
using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.PetSpecies;
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
		private readonly IMapper _mapper;

		public PetSpeciesManagerService(IUnitOfWork unitOfWork, IMapper mapper)
		{
			_unitOfWork = unitOfWork;
			_mapper = mapper;
		}
		public async Task<ResponseData<string>> Create(PetSpeciesCreateUpdate request)
		{
			try
			{
				var data = new PetSpecies()
				{
					Name = request.Name,
					PetTypeId = request.PetTypeId,
					IsDelete = request.IsDelete,
				};
				 await _unitOfWork.PetSpeciesRepository.AddAsync(data);
				var result = await _unitOfWork.SaveChangeAsync();
				if (result>0)
                {                    
					return new ResponseData<string>("Thêm thành công");
                }
				return new ResponseData<string>()
				{
					IsSuccess = false,
					Error = "Có lỗi kết nối vs sql"
				};

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

		public async Task<ResponseData<PetSpecies>> FindPetSpeciesByID(int id)
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

		public async Task<ResponseData<string>> SoftDelete(DeleteRequest<int> request)
		{
			try
			{
				var data = await _unitOfWork.PetSpeciesRepository.GetAsync(request.ID);
				data.IsDelete= request.IsDelete;
				await _unitOfWork.PetSpeciesRepository.UpdateAsync(data);
				var response = await _unitOfWork.PetSpeciesRepository.SaveChangesAsync();
				if (response>0)
                {                
					   return new ResponseData<string>("Thay đổi trạng thái thành công");
                }
				return new ResponseData<string>("Thay đổi trạng thái không thành công");

			}
			catch (Exception)
			{
				return new ResponseData<string>()
				{
					IsSuccess = false,
					Error = "Có lỗi khi lấy dữ liệu"
				};
			}
		}

		public async Task<ResponseData<string>> Update(PetSpeciesCreateUpdate request)
		{

			try
			{
				var data = new PetSpecies()
				{
					Name = request.Name,
					PetTypeId = request.PetTypeId,
					IsDelete = request.IsDelete,
					Id = request.Id,
				}; 
				 await _unitOfWork.PetSpeciesRepository.UpdateAsync(data);
				var result = await _unitOfWork.SaveChangeAsync();
				if (result > 0)
				{
					return new ResponseData<string>("Cập nhật thành công");
				}
				return new ResponseData<string>()
				{
					IsSuccess = false,
					Error = "Có lỗi kết nối vs sql"
				};

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
