using AutoMapper;
using Azure.Core;
using DATN.Aplication.Extentions;
using DATN.Aplication.Repository;
using DATN.Aplication.Repository.IRepository;
using DATN.Data.EF;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Guest;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
    public class GuestManagerService : IGuestManagerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private PasswordExtensitons _passwordExtensitons;
        private MailExtention _mailExtension;
        public GuestManagerService(IMapper mapper, IUnitOfWork unitOfWork, MailExtention mailExtension)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _passwordExtensitons = new PasswordExtensitons();
            _mailExtension = mailExtension;
        }

        public async Task<ResponseData<GuestViewModel>> FindGuestByID(Guid id)
        {

            try
            {
                var result = await _unitOfWork.GuestRepository.GetAsync(id);

                if (result != null)
                {
                    var guestVM = _mapper.Map<GuestViewModel>(result);
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

        public async Task<ResponseData<List<GuestViewModel>>> GetGuest()
        {
            try
            {
                var result = await _unitOfWork.GuestRepository.GetAllAsync();
                var lstPet = await _unitOfWork.PetRepository.GetAllAsync();
                var response = result.Where(p => p.IsComfirm == true).OrderByDescending(p => p.RegisteredAt).Select(p => new GuestViewModel()
                {
                    Address = p.Address,
                    Email = p.Email,
                    Gender = p.Gender,
                    Id = p.Id,
                    Name = p.Name,
                    IsDelete = p.IsDeleted,
                    Password = p.PasswordHash != null ? _passwordExtensitons.Decrypt(p.PasswordHash) : "",
                    PhoneNumber = p.PhoneNumber,
                    UserName = p.UserName,
                }).ToList();
                foreach (var item in response)
                {
                    item.CountPet = lstPet.Where(p => p.OwnerId == item.Id).Count();
                }
                int totalCount = result.Count();
                if (totalCount != 0)
                {

                    return new ResponseData<List<GuestViewModel>>
                    {
                        IsSuccess = true,
                        Data = response,
                    };
                }
                return new ResponseData<List<GuestViewModel>>
                {
                    IsSuccess = true,
                    Data = new List<GuestViewModel>()
                };
            }

            catch (Exception ex)
            {

                return new ResponseData<List<GuestViewModel>>
                {
                    IsSuccess = false,
                    Error = ex.Message
                };
            }
        }

        public async Task<ResponseData<string>> RegisterByCustomer(GuestRegisterUserRequest request)
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
                    IsComfirm = false,
                };
                await _unitOfWork.GuestRepository.AddAsync(guest);
                var result = await _unitOfWork.SaveChangeAsync();
                await _mailExtension.SendMailVerificationAsync(guest.Email, guest.UserName, request.Password, guest.Id.ToString());
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

        public async Task<ResponseData<string>> RegisterNoUser(GuestRegisterNoUserRequest request)
        {
            try
            {
                var checkMail= _unitOfWork.GuestRepository.CheckEmailExist(request.Email);
                if (checkMail != null)
                {
                    return new ResponseData<string>(false, "Tài khoản email đã được đăng ký trước đó");
                }
                string randomPass = "";
                bool createUser = !string.IsNullOrEmpty(request.Email);
                if (createUser)
                {
                    randomPass = GeneratePassword();
                }
                var guest = new Guest()
                {
                    Address = request.Address,
                    Name = request.Name,
                    PhoneNumber = request.PhoneNumber,
                    Gender = request.Gender,
                    Email = request.Email,
                    UserName = request.Email,
                    PasswordHash = randomPass != "" ? _passwordExtensitons.HashPassword(randomPass) : null,
                    IsComfirm = !createUser,
                    IsDeleted = false,
                    RegisteredAt = DateTime.Now,
                };
                await _unitOfWork.GuestRepository.AddAsync(guest);
                var result = await _unitOfWork.SaveChangeAsync();
                if (createUser)
                {
                    //	await _mailExtension.SendMailVerificationAsync(guest.Email, guest.UserName, randomPass, guest.Id.ToString());
                }
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
                string erorrMessage = "";
                bool checkUser = true;
                if (await _unitOfWork.GuestRepository.CheckEmailExist(request.Email))
                {
                    checkUser = false;
                    erorrMessage = "Email đã được đăng ký ở tài khoản khác";
                }
                if (await _unitOfWork.GuestRepository.CheckUserExist(request.UserName))
                {
                    checkUser = false;
                    erorrMessage = "Tên đăng nhập đã tồn tại";
                }
                if (!checkUser)
                {
                    return new ResponseData<string>
                    {
                        IsSuccess = false,
                        Error = erorrMessage,
                    };
                }


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
                    IsComfirm = false,
                };
                await _unitOfWork.GuestRepository.AddAsync(guest);
                var result = await _unitOfWork.SaveChangeAsync();


                await _mailExtension.SendMailVerificationAsync(guest.Email, guest.UserName, request.Password, guest.Id.ToString());
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

        public async Task<ResponseData<string>> SoftDelete(DeleteRequest<Guid> request)
        {
            try
            {
                var result = await _unitOfWork.GuestRepository.SoftDelete(request);
                if (result)
                {
                    return new ResponseData<string>
                    {
                        IsSuccess = true,
                        Data = "Thay đôi trạng thái thành công"
                    };
                }
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Data = "Thay đôi trạng thái không thành công"
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

        public async Task<ResponseData<string>> UpdateGuest(GuestUpdateRequest request)
        {
            try
            {
                var guest = await _unitOfWork.GuestRepository.GetAsync(request.Id);
                if (guest == null)
                {
                    return new ResponseData<string>
                    {
                        IsSuccess = false,
                        Error = "Sửa thất bại"
                    };
                }

                guest.Address = request.Address;
                guest.Gender = request.Gender;
                guest.Name = request.Name;
                guest.IsDeleted = false;
                guest.PhoneNumber = request.PhoneNumber;



                await _unitOfWork.GuestRepository.UpdateAsync(guest);
                var result = await _unitOfWork.SaveChangeAsync();

                if (result > 0)
                {
                    return new ResponseData<string>
                    {
                        IsSuccess = true,
                        Data = " Sửa thành công"
                    };
                }
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = "Sửa thất bại"
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

        public async Task<ResponseData<string>> VerififyUser(string verifyConstring, string mail)
        {
            try
            {

                var guest = await _unitOfWork.GuestRepository.GetAsync(Guid.Parse(verifyConstring));
                if (guest != null && guest.Email == mail)
                {
                    guest.IsComfirm = true;
                    var result = await _unitOfWork.SaveChangeAsync();

                    if (result > 0)
                    {
                        return new ResponseData<string>
                        {
                            IsSuccess = true,
                            Data = " Xác minh thành công"
                        };
                    }
                }

                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = "Xác minh thất bại thất bại"
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

        private string GeneratePassword()
        {
            int passLength = 8;
            const string upperCase = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            const string lowerCase = "abcdefghijklmnopqrstuvwxyz";
            const string digits = "0123456789";
            const string specialChars = "!@#$%^&*()_+<>?";

            string allChars = upperCase + lowerCase + digits + specialChars;
            Random random = new Random();

            StringBuilder password = new StringBuilder();

            // Add one random character from each category to ensure the password contains at least one of each
            password.Append(upperCase[random.Next(upperCase.Length)]);
            password.Append(lowerCase[random.Next(lowerCase.Length)]);
            password.Append(digits[random.Next(digits.Length)]);
            password.Append(specialChars[random.Next(specialChars.Length)]);

            // Fill the rest of the password length with random characters
            for (int i = 4; i < passLength; i++)
            {
                password.Append(allChars[random.Next(allChars.Length)]);
            }

            // Shuffle the password to remove any predictable pattern
            return ShuffleString(password.ToString());
        }

        private string ShuffleString(string input)
        {
            char[] array = input.ToCharArray();
            Random random = new Random();
            int n = array.Length;
            for (int i = n - 1; i > 0; i--)
            {
                int j = random.Next(i + 1);
                char temp = array[i];
                array[i] = array[j];
                array[j] = temp;
            }
            return new string(array);
        }

    }
}
