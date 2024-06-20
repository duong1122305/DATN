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
				var guest = new Guest()
				{
					Address = request.Address,
					Name = request.Name,
					PhoneNumber = request.PhoneNumber,
					Gender = request.Gender,
					Email = request.Email,
					IsComfirm = true,
					IsDeleted = false,
					RegisteredAt = DateTime.Now,
				};
				await _unitOfWork.GuestRepository.AddAsync(guest);
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
					IsSuccess = true,
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


	}
}
