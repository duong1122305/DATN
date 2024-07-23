using AutoMapper;
using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Pet;

namespace DATN.Aplication.Services
{
    public class PetManagerService : IPetManagerService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public PetManagerService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }
        public async Task<ResponseData<List<PetVM>>> GetPetByGuestId(Guid guestId)
        {
            try
            {
                var lstPet = await _unitOfWork.PetRepository.GetAllAsync();
                var lstPetSpecies = await _unitOfWork.PetSpeciesRepository.GetAllAsync();
                var lstType = await _unitOfWork.PetTypeRepository.GetAllAsync();
                var lstPetVm = from pet in lstPet
                               join petSpecies in lstPetSpecies on pet.SpeciesId equals petSpecies.Id
                               join petType in lstType on petSpecies.PetTypeId equals petType.Id
                               where pet.OwnerId == guestId
                               select (new PetVM()
                               {
                                   Birthday = pet.Birthday,
                                   Gender = pet.Gender,
                                   PetTypeId = petSpecies.PetTypeId,
                                   Id = pet.Id,
                                   Name = pet.Name,
                                   Neutered = pet.Neutered,
                                   Note = pet.Note,
                                   OriginalColor = pet.OriginalColor,
                                   OwnerId = pet.OwnerId,
                                   SpeciesId = pet.SpeciesId,
                                   Vaccinated = pet.Vaccinated,
                                   Weight = pet.Weight,
                                   Species = petSpecies.Name,
                               });

                return new ResponseData<List<PetVM>>(lstPetVm.ToList());
            }
            catch (Exception)
            {

                return new ResponseData<List<PetVM>>()
                {
                    IsSuccess = false,
                    Error = "Lỗi khi truy cập máy chủ"
                };
            }
        }
        public async Task<ResponseData<List<PetVM>>> GetPetBySpeciesId(int id)
        {
            try
            {
                var lstPet = await _unitOfWork.PetRepository.GetAllAsync();
                var lstGuest = await _unitOfWork.GuestRepository.GetAllAsync();
                var lstPetVm = from pet in lstPet
                               join guest in lstGuest on pet.OwnerId equals guest.Id
                               where pet.SpeciesId == id
                               select (new PetVM()
                               {
                                   Birthday = pet.Birthday,
                                   Gender = pet.Gender,
                                   Id = pet.Id,
                                   Name = pet.Name,
                                   Neutered = pet.Neutered,
                                   Note = pet.Note,
                                   OriginalColor = pet.OriginalColor,
                                   OwnerId = pet.OwnerId,
                                   SpeciesId = pet.SpeciesId,
                                   Vaccinated = pet.Vaccinated,
                                   Weight = pet.Weight,
                                   Owner = guest.Name,
                                   Address = guest.Address,
                               });

                return new ResponseData<List<PetVM>>(lstPetVm.ToList());
            }
            catch (Exception)
            {

                return new ResponseData<List<PetVM>>()
                {
                    IsSuccess = false,
                    Error = "Lỗi khi truy cập máy chủ"
                };
            }
        }
        public async Task<ResponseData<string>> CreatePet(PetCreateUpdate petVM)
        {
            try
            {
                var pet = _mapper.Map<Pet>(petVM);
                await _unitOfWork.PetRepository.AddAsync(pet);
                var reusult = await _unitOfWork.SaveChangeAsync();
                if (reusult > 0)
                {
                    return new ResponseData<string>("Thêm thành công");
                }
                return new ResponseData<string>(false, "Lỗi khi lưu vào csdl");
            }
            catch (Exception ex)
            {

                return new ResponseData<string>(false, "Có lỗi xảy ra: " + ex);
            }
        }
        public async Task<ResponseData<string>> UpdatePet(PetCreateUpdate petVM)
        {
            try
            {
                var pet = _mapper.Map<Pet>(petVM);
                await _unitOfWork.PetRepository.UpdateAsync(pet);
                var reusult = await _unitOfWork.SaveChangeAsync();
                if (reusult > 0)
                {
                    return new ResponseData<string>("Thêm thành công");
                }
                return new ResponseData<string>(false, "Lỗi khi lưu vào csdl");
            }
            catch (Exception ex)
            {

                return new ResponseData<string>(false, "Có lỗi xảy ra: " + ex);
            }
        }

        public async Task<ResponseData<string>> SoftDelete(DeleteRequest<int> request)
        {
            try
            {
                var pet = await _unitOfWork.PetRepository.GetAsync(request.ID);
                if (pet != null)
                {
                    pet.IsDelete = request.IsDelete;
                }
                var reusult = await _unitOfWork.SaveChangeAsync();
                if (reusult > 0)
                {
                    return new ResponseData<string>("Xoá thành công");
                }
                return new ResponseData<string>(false, "Lỗi khi lưu vào csdl");
            }
            catch (Exception ex)
            {

                return new ResponseData<string>(false, "Có lỗi xảy ra: " + ex);
            }
        }

        public async Task<ResponseData<List<PetVM>>> GetAll()
        {
            try
            {
                var lstPet = await _unitOfWork.PetRepository.GetAllAsync();
                var lstPetVM = _mapper.Map<List<PetVM>>(lstPet);
                return new ResponseData<List<PetVM>>(lstPetVM);
            }
            catch (Exception ex)
            {

                return new ResponseData<List<PetVM>>()
                {
                    IsSuccess = false,
                    Error = "Lỗi khi truy cập máy chủ"
                };
            }
        }

        public async Task<ResponseData<List<PetType>>> GetAllTypes()
        {
            try
            {
                var lst = await _unitOfWork.PetTypeRepository.GetAllAsync();
                return new ResponseData<List<PetType>>()
                {
                    IsSuccess = true,
                    Data = lst.ToList()
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<List<PetType>>()
                {
                    IsSuccess = false,
                    Error = "Lỗi khi truy cập máy chủ" + ex.Message
                };
            }
        }

    }
}
