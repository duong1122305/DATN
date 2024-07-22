using AutoMapper;
using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.PetSpecies;

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
        private async Task<bool> PetNameIsExist(string name, int id)
        {
            var result = await _unitOfWork.PetSpeciesRepository.FindAsync(p => p.Name.ToLower() == name.ToLower() && p.Id != id);
            if (result == null || result.Count() < 1)
            {
                return false;
            }
            return true;

        }
        public async Task<ResponseData<string>> Create(PetSpeciesCreateUpdate request)
        {
            try
            {
                if (await PetNameIsExist(request.Name, request.Id))
                {
                    return new ResponseData<string>(false, "Tên pet đã tồn tại");
                }
                var data = new PetSpecies()
                {
                    Name = request.Name,
                    PetTypeId = request.PetTypeId,
                    IsDelete = request.IsDelete,
                };
                await _unitOfWork.PetSpeciesRepository.AddAsync(data);
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
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

        public async Task<ResponseData<PetSpeciesVM>> FindPetSpeciesByID(int id)
        {
            try
            {
                var data = await _unitOfWork.PetSpeciesRepository.GetAsync(id);
                var listPet = await _unitOfWork.PetRepository.GetAllAsync();
                var response = new PetSpeciesVM()
                {
                    Name = data.Name,
                    Id = data.Id,
                    IsDelete = data.IsDelete,
                    PetTypeId = data.PetTypeId,
                    PetPype = data.PetTypeId == 1 ? "Chó" : "Mèo",
                    CountPet = listPet.Count(q => q.SpeciesId == data.Id)
                };
                return new ResponseData<PetSpeciesVM>(response);

            }
            catch (Exception)
            {
                return new ResponseData<PetSpeciesVM>()
                {
                    IsSuccess = false,
                    Error = "Có lỗi khi lấy dữ liệu"
                };
            }
        }

        public async Task<ResponseData<List<PetSpeciesVM>>> GetAll()
        {
            try
            {
                var data = await _unitOfWork.PetSpeciesRepository.GetAllAsync();
                var listPet = await _unitOfWork.PetRepository.GetAllAsync();
                var response = data.Select(p => new PetSpeciesVM()
                {
                    Name = p.Name,
                    Id = p.Id,
                    IsDelete = p.IsDelete,
                    PetTypeId = p.PetTypeId,
                    PetPype = p.PetTypeId == 1 ? "Chó" : "Mèo",
                    CountPet = listPet.Count(q => q.SpeciesId == p.Id)
                }).OrderByDescending(p => p.Id).ToList();

                return new ResponseData<List<PetSpeciesVM>>(response);

            }
            catch (Exception)
            {
                return new ResponseData<List<PetSpeciesVM>>()
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
                data.IsDelete = request.IsDelete;
                await _unitOfWork.PetSpeciesRepository.UpdateAsync(data);
                var response = await _unitOfWork.PetSpeciesRepository.SaveChangesAsync();
                if (response > 0)
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
                request.Name = request.Name.Trim();
                if (await PetNameIsExist(request.Name, request.Id))
                {
                    return new ResponseData<string>(false, "Tên loài đã tồn tại");
                }
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
