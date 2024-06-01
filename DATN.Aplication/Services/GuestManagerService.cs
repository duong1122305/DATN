using AutoMapper;
using Azure.Core;
using DATN.Aplication.Extentions;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Guest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
    public class GuestManagerService: IGuestManagerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private PasswordExtensitons _passwordExtensitons;
        public GuestManagerService(IMapper mapper, IUnitOfWork unitOfWork)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _passwordExtensitons = new PasswordExtensitons();
        }

        public async Task<ResponseData<GuestViewModel>> FindGuestByID(Guid id)
        {
          
            try
            {
                var result = _unitOfWork.GuestRepository.GetAsync(id);

                if (result !=null)
                {
                    var guestVM= _mapper.Map<GuestViewModel>(result);
                    return new ResponseData<GuestViewModel>
                    {
                        IsSuccess = true,
                        Data = guestVM
                    };
                }
                return new ResponseData<GuestViewModel>
                {
                    IsSuccess = false,
                    Error = "Không tìm thấy khách hàng"
                };
            }
            catch (Exception ex)
            {

                return new ResponseData<GuestViewModel>
                {
                    IsSuccess = false,
                    Error = ex.Message
                };
            }
        }

        public async Task<ResponseData<GetGuestResponse>> GetGuest(GetGuestRequest request)
        {
            try
            {
                var result = await _unitOfWork.GuestRepository.GetAllAsync();

                if (result != null)
                {
                    var guestVM = _mapper.Map<GuestViewModel>(result);
                    return new ResponseData<GetGuestResponse>
                    {
                        IsSuccess = true,
                        Data = guestVM
                    };
                }
                return new ResponseData<GetGuestResponse>
                {
                    IsSuccess = false,
                    Error = "Không tìm thấy khách hàng"
                };
            }
            catch (Exception ex)
            {

                return new ResponseData<GetGuestResponse>
                {
                    IsSuccess = false,
                    Error = ex.Message
                };
            }
        }

        public Task<ResponseData<string>> RegisterByCustomer(GuestRegisterUserRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<ResponseData<string>> RegisterNoUser(GuestRegisterNoUserRequest request)
        {
            try
            {
                var guest = new Guest()
                {
                    Address = request.Address,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    Gender = request.Gender,
                };
                var result = await _unitOfWork.SaveChangeAsync();
                if (result > 0)
                {
                    return new ResponseData<string>
                    {
                        IsSuccess = true,
                        Data = " Thêm thành công"
                    };
                }
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = "Thêm thất bại"
                };
            }
            catch (Exception ex)
            {

                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = ex.Message
                };
            }
        }

        public async Task<ResponseData<string>> RegisterWithUser(GuestRegisterUserRequest request)
        {

            try
            {
                var guest = new Guest()
                {
                    Address = request.Address,
                    Email = request.Email,
                    Gender = request.Gender,
                    Name = request.Name,
                    IsDeleted = false,
                    PhoneNumber = request.PhoneNumber,
                    RegisteredAt = DateTime.Now,
                    UserName = request.UserName,
                    PasswordHash = _passwordExtensitons.HashPassword(request.Password),
                };
                await _unitOfWork.GuestRepository.AddAsync(guest);
                var result=  await _unitOfWork.SaveChangeAsync();
                if (result>0)
                {
                    return new ResponseData<string>
                    {
                        IsSuccess = true,
                        Data=" Thêm thành công"
                    };
                }
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = "Thêm thất bại"
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = ex.Message
                };
            }
        }

        public Task<ResponseData<string>> UpdateGuest(GuestViewModel request)
        {
            throw new NotImplementedException();
        }
    }
}
