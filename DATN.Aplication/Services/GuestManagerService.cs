﻿using AutoMapper;
using CloudinaryDotNet;
using DATN.API.Services;
using DATN.Aplication.Extentions;
using DATN.Data.Entities;
using DATN.Utilites;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Guest;

namespace DATN.Aplication.Services
{

    public class GuestManagerService : IGuestManagerService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private PasswordExtensitons _passwordExtensitons;
        private MailExtention _mailExtension;
        private readonly IUploadFileServices _cloundinary;
        public GuestManagerService(IMapper mapper, IUnitOfWork unitOfWork, MailExtention mailExtension, IUploadFileServices cloudinary)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _passwordExtensitons = new PasswordExtensitons();
            _mailExtension = mailExtension;
            _cloundinary = cloudinary;
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

		public async Task<ResponseData<List<GuestViewModel>>> GetAllGuest()
		{
			try
			{
				var result = await _unitOfWork.GuestRepository.GetAllAsync();
				var response = result.Where(p=>p.IsComfirm.Value&& !(p.IsDeleted.HasValue&&p.IsDeleted.Value)).Select(p => new GuestViewModel()
				{
					Address = p.Address,
					Email = p.Email,
					Gender = p.Gender,
					Id = p.Id,
					Name = p.Name,
					IsDelete = p.IsDeleted,
					PhoneNumber = p.PhoneNumber,
					UserName = p.UserName,
					IsConfirm = p.IsComfirm
				}).ToList();
				
				if (response != null && response.Count>0)
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
		public async Task<ResponseData<List<GuestViewModel>>> GetGuest()
        {
            try
            {
                var result = await _unitOfWork.GuestRepository.GetAllAsync();
                var lstPet = await _unitOfWork.PetRepository.GetAllAsync();
                var response = result.Where(x => x.Id != Contant.GuestsID).OrderByDescending(p => p.RegisteredAt).Select(p => new GuestViewModel()
                {
                    Address = p.Address,
                    Email = p.Email,
                    Gender = p.Gender,
                    Id = p.Id,
                    Name = p.Name,
                    IsDelete = p.IsDeleted,
                    PhoneNumber = p.PhoneNumber,
                    UserName = p.UserName,
                    IsConfirm = p.IsComfirm
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
        public async Task<ResponseData<string>> CheckAndSendMail(string email)
        {
            try
            {
                if (await _unitOfWork.GuestRepository.CheckEmailExist(email))
                {
                    return new ResponseData<string>(false, "Email đã xem được sử dụng");
                }
                var verifyCode = RandomCodeExtention.GennarateVerifyCode(Guid.NewGuid().ToString());//tạo mã xác mình
                await _mailExtension.SendMailVerificationGuestAsync(email, verifyCode);
                return new ResponseData<string>(verifyCode);
            }
            catch (Exception ex)
            {

                return new ResponseData<string>(false, "Có lỗi ở máy chủ: " + ex.Message);
            }
        }
        public async Task<ResponseData<string>> RegisterNoUser(GuestRegisterByGuestRequest request)
        {
            throw new NotImplementedException();
            //try
            //{

            //    string erorrMessage = "";
            //    bool checkUser = true;
            //    if (!string.IsNullOrEmpty(request.Email))
            //    {
            //        if (await _unitOfWork.GuestRepository.CheckEmailExist(request.Email))
            //        {
            //            checkUser = false;
            //            erorrMessage = "Email đã được đăng ký ở tài khoản khác";
            //        }
            //        if (await _unitOfWork.GuestRepository.CheckPhoneNumberExist(request.PhoneNumber))
            //        {
            //            checkUser = false;
            //            erorrMessage = "Số điện thoiaj đã được đăng ký ở tài khoản khác";
            //        }
            //        if (!checkUser)
            //        {
            //            return new ResponseData<string>
            //            {
            //                IsSuccess = false,
            //                Error = erorrMessage,
            //            };
            //        }
            //    }
            //    var guest = new Guest()
            //    {
            //        Address = request.Address,
            //        Email = request.Email,
            //        Gender = request.Gender,
            //        Name = request.Name,
            //        IsDeleted = false,
            //        PhoneNumber = request.PhoneNumber,
            //        RegisteredAt = DateTime.Now,
            //        UserName = request.UserName,
            //        PasswordHash = _passwordExtensitons.HashPassword(request.Password),
            //        IsComfirm = true,
            //    };
            //    await _unitOfWork.GuestRepository.AddAsync(guest);
            //    var result = await _unitOfWork.SaveChangeAsync();
            //    if (result > 0)
            //    {
            //        return new ResponseData<string>
            //        {
            //            IsSuccess = true,
            //            Data = " Thêm thành công"
            //        };
            //    }
            //    return new ResponseData<string>
            //    {
            //        IsSuccess = false,
            //        Error = "Thêm thất bại"
            //    };
            //}
            //catch (Exception ex)
            //{
            //    return new ResponseData<string>
            //    {
            //        IsSuccess = false,
            //        Error = ex.Message
            //    };
            //}
        }
        public async Task<ResponseData<string>> RegisterByCustomer(GuestRegisterByGuestRequest request)
        {
            try
            {
                bool hasEmail = !string.IsNullOrEmpty(request.Email);

                if (await _unitOfWork.GuestRepository.CheckEmailExist(request.Email))
                {
                    return new ResponseData<string>
                    {
                        IsSuccess = false,
                        Error = "Email đã được đăng ký ở tài khoản khác"
                    };
                } 
                if (await _unitOfWork.GuestRepository.CheckPhoneNumberExist(request.PhoneNumber))
                {
                    return new ResponseData<string>
                    {
                        IsSuccess = false,
                        Error = "Số điện thoại đã được đăng ký ở tài khoản khác"
                    };
                }

                var guest = new Guest();
                guest = await _unitOfWork.GuestRepository.FindByEmail(request.Email);
                if (guest == null || guest.Name == null)
                {
                    guest = new Guest()
                    {
                        Address = request.Address,
                        Email = request.Email,
                        Gender = request.Gender,
                        Name = request.Name,
                        IsDeleted = false,
                        PhoneNumber = request.PhoneNumber,
                        RegisteredAt = DateTime.Now,
                        UserName = request.UserName,
                        PasswordHash = !hasEmail ? null : _passwordExtensitons.HashPassword(request.Password),
                        IsComfirm = !hasEmail,
                    };

                    if (request.AvatarFile != null)
                    {
                        var uploadFile = await _cloundinary.UploadAvatarAsync(request.AvatarFile);
                        if (uploadFile.IsSuccess)
                        {
                            guest.AvatarUrl = uploadFile.Data;
                        }
                        else
                        {
                            return new ResponseData<string>
                            {
                                IsSuccess = false,
                                Error = "Upload ảnh thất bại"
                            };
                        }
                    }
                    await _unitOfWork.GuestRepository.AddAsync(guest);
                }
                else
                {
                    guest.Name = request.Name;
                    guest.Address = request.Address;
                    guest.Email = request.Email;
                    guest.Gender = request.Gender;
                    guest.RegisteredAt = DateTime.Now;
                    guest.PasswordHash = _passwordExtensitons.HashPassword(request.Password);
                    await _unitOfWork.GuestRepository.UpdateAsync(guest);
                }

                if (hasEmail)
                {
                    var verifyCode = RandomCodeExtention.GennarateVerifyCode(guest.Id.ToString());//tạo mã xác mình
                    guest.VerifyCode = verifyCode;
                    await _mailExtension.SendMailVerificationGuestAsync(request.Email, verifyCode);
                }
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
                string randomPass = "";
                bool hasEmail = !string.IsNullOrEmpty(request.Email);

                if (hasEmail && await _unitOfWork.GuestRepository.CheckEmailExist(request.Email))
                {
                    return new ResponseData<string>
                    {
                        IsSuccess = false,
                        Error = "Email đã được đăng ký ở tài khoản khác"
                    };
                }
                if (await _unitOfWork.GuestRepository.CheckPhoneNumberExist(request.PhoneNumber))
                {
                    return new ResponseData<string>
                    {
                        IsSuccess = false,
                        Error = "Số điện thoại đã được đăng ký ở tài khoản khác"
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
                    UserName = request.Email,
                    PasswordHash = hasEmail ? _passwordExtensitons.HashPassword(randomPass) : null,
                    IsComfirm = !hasEmail,

                };
                await _unitOfWork.GuestRepository.AddAsync(guest);

                if (hasEmail)
                {
                    var verifyCode = RandomCodeExtention.GennarateVerifyCode(guest.Id.ToString());//tạo mã xác mình
                    guest.VerifyCode = verifyCode;
                    await _mailExtension.SendMailVerificationAsync(guest.Email, guest.UserName, randomPass, verifyCode);
                }
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

        public async Task<ResponseData<string>> SendForgotMail(string mail)
        {
            try
            {

                var guest = await _unitOfWork.GuestRepository.FindByEmailAll(mail.Trim());

                if (guest != null)
                {

                    string verifyCode = RandomCodeExtention.RandomCodeOnlyNumber();
                    guest.VerifyCode = verifyCode+"|"+ DateTime.Now.AddMinutes(5).ToString();
                    await _unitOfWork.GuestRepository.UpdateAsync(guest);
                    var result = await _unitOfWork.SaveChangeAsync();
                    if (result == 0)
                    {
                        return new ResponseData<string>
                        {
                            IsSuccess = false,
                            Error = "Có gì đó sai sai"
                        };
                    }
                    await _mailExtension.SendMailGuestForgot(guest.Email!, guest.VerifyCode.Split("|").First());

                }
                return new ResponseData<string>
                {
                    IsSuccess = true,
                    Data = "Tin nhắn đã gửi vào mail của quý khách"
                };



            }
            catch (Exception ex)
            {
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = "Có lỗi xảy ra: " + ex.Message
                };
            }
        }
        public async Task<ResponseData<string>> VerififyUser(string verifyCode)
        {
            try
            {
                string verifyString = _passwordExtensitons.DeCode(verifyCode);// giải mã thông tin xác minh
                string[] dataVerify = verifyString.Split('|');// [0] id khách // [1] mã thời hạn// [2] mã bảo mật thêm

                var guest = await _unitOfWork.GuestRepository.GetAsync(Guid.Parse(dataVerify[0]));
                if (guest != null && guest.VerifyCode == verifyCode)
                {
                    if (DateTime.Parse(dataVerify[1]) < DateTime.Now)// kiểm tra thời gian phù hợp vs max
                    {
                        return new ResponseData<string>
                        {
                            IsSuccess = false,
                            Error = "Thông tin xác minh của bạn đã quá hạn"
                        };
                    }
                    guest.IsComfirm = true;
                    guest.VerifyCode = null;
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
                    Error = "Tài khoản đã được xác minh hoặc mã xác minh sai"
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = "Có lỗi trong quá trình xác minh"
                };
            }
        }


        public async Task<ResponseData<string>> ChangPassWithVerifyCode(string verifyCode, string newPass)
        {
            try
            {
                var checkPass = _passwordExtensitons.ValidatePassword(newPass);
                if (!checkPass.IsSuccess)
                {
                    return checkPass;
                }
                string verifyString = _passwordExtensitons.DeCode(verifyCode);// giải mã thông tin xác minh
                string[] dataVerify = verifyString.Split('|');// [0] id khách // [1] mã thời hạn

                var guest = await _unitOfWork.GuestRepository.GetAsync(Guid.Parse(dataVerify[0]));
                if (guest != null)
                {
                    if (DateTime.Parse(dataVerify[1]) < DateTime.Now||  string.IsNullOrEmpty(guest.VerifyCode))// kiểm tra thời gian phù hợp vs max
                    {
                        return new ResponseData<string>
                        {
                            IsSuccess = false,
                            Error = "Thông tin xác minh của bạn đã quá hạn"
                        };
                    }
                    guest.VerifyCode = "";
                    guest.PasswordHash = _passwordExtensitons.HashPassword(newPass);
                    var result = await _unitOfWork.SaveChangeAsync();

                    if (result > 0)
                    {
                        return new ResponseData<string>
                        {
                            IsSuccess = true,
                            Data = " Thay đổi mật khẩu thành công"
                        };
                    }
                }

                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = "Thay đôi mật khẩu thất bại"
                };
            }
            catch (Exception ex)
            {
                return new ResponseData<string>
                {
                    IsSuccess = false,
                    Error = "Có lỗi trong quá trình thay đổi mật khẩu"
                };
            }
        }

        public async Task<ResponseData<string>> CheckConfirmCode(string confirmCode, string email)
        {
            try
            {
                var guests = await _unitOfWork.GuestRepository.FindAsync(p=>p.Email == email&& (!p.IsDeleted.HasValue || !p.IsDeleted.Value)&& p.IsComfirm.Value);
                if (guests != null && guests.Count() >0)
                {
                    var guest= guests.First();
                    var confirmAndTime = guest.VerifyCode;
                    string[] arrData = confirmAndTime.Split("|");
                    if (arrData[0] != confirmCode)
                    {
                        return new ResponseData<string>(false, "Thông tin xác minh của bạn không chính xác");
                    }
                    else if (DateTime.Parse(arrData[1])<DateTime.Now){
                        return new ResponseData<string>(false, "Thông tin xác minh của bạn đã quá hạn");
                    }
                    guest.VerifyCode= RandomCodeExtention.GennarateVerifyCode(guest.Id.ToString());
                    await _unitOfWork.GuestRepository.UpdateAsync(guest);
                    var result=  await _unitOfWork.SaveChangeAsync();
                    if (result>0)
                    {
                        return new ResponseData<string>(guest.VerifyCode);
                    }

                }
                return new ResponseData<string> ( false, "Thông tin xác minh của bạn không chính xác" );
            }
            catch (Exception)
            {

                return new ResponseData<string>(false, "Có lỗi xảy ra trong quá trình xác minh");
            }
        }
    }
}
