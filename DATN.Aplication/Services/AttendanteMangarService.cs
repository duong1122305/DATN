using DATN.Aplication.CustomProvider;
using DATN.Aplication.Extentions;
using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Attendace;
using DATN.ViewModels.Enum;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using static System.Collections.Specialized.BitVector32;

namespace DATN.Aplication.Services
{
	public class AttendanteMangarService : IAttendanteMangarService
	{
		private readonly IUnitOfWork _unitOfWork;
		private readonly UserManager<User> _userManager;
		private readonly IHttpContextAccessor _httpContextAccessor;
		private PasswordExtensitons _passwordExtensitons= new PasswordExtensitons();
		public AttendanteMangarService(IUnitOfWork unitOfWork, UserManager<User> userManager, IHttpContextAccessor httpContextAccessor)
		{
			_unitOfWork = unitOfWork;
			_userManager = userManager;
			_httpContextAccessor = httpContextAccessor;
		}
		public async Task<ResponseData<List<AttendanceViewModel>>> ListAttendanceToday(int shiftID)
		{
			try
			{
				
				var lstWorkShiftToday = await _unitOfWork.WorkShiftRepository.FindAsync(p => p.WorkDate.Date == DateTime.Today.Date);
				var lstSchedule = await _unitOfWork.EmployeeScheduleRepository.GetAllAsync();
				var lstAttendance = await _unitOfWork.EmployeeAttendanceRepository.GetAllAsync();
				var lstUser = _userManager.Users.ToList();
				var result = from user in lstUser
							 join st in lstSchedule on user.Id equals st.UserId
							 join ws in lstWorkShiftToday on st.WorkShiftId equals ws.Id
							 join at in lstAttendance on st.Id equals at.EmployeeScheduleId into attendanceData
							 from at in attendanceData.DefaultIfEmpty()
							 where ws.ShiftId == shiftID 
							 select new AttendanceViewModel
							 {
								 UserName = user.UserName,
								 DateAttendace = DateTime.Today.ToString("dd/MM/yyyy"),
								 CheckInTime = at != null && at.CheckInTime.HasValue ? at.CheckInTime.Value.ToString("hh:mm") : "0",
								 CheckOutTime = at != null && at.CheckOutTime.HasValue ? at.CheckOutTime.Value.ToString("hh:mm") : "0",
								 ID = at != null ? at.Id : 0,
								 StaffName = user.FullName,
								 ScheduleID = st.Id,
							 };
				var data = result.ToList();
				if (result == null || result.Count() == 0)
				{
					return new ResponseData<List<AttendanceViewModel>>(false, "Không có lịch làm việc lúc này");
				}
				return new ResponseData<List<AttendanceViewModel>>(result.ToList());
			}
			catch (Exception)
			{

				return new ResponseData<List<AttendanceViewModel>>(false, "Đại vương ơi có bug rồi");
			}
		}
		public async Task<ResponseData<List<Shift>>> GetShiftNow()
		{
			try
			{
				var now = DateTime.Now.TimeOfDay;
				var lstShift = await _unitOfWork.ShiftRepository.GetAllAsync();
				lstShift = lstShift.Where(p => p.From.Add(TimeSpan.FromMinutes(-15)) < now && p.To > now);
				if (lstShift == null || lstShift.Count() == 0)
				{
					return new ResponseData<List<Shift>>(false, "Hiện tại không trong ca làm");
				}
				return new ResponseData<List<Shift>>(lstShift.ToList());
			}
			catch (Exception)
			{
				return new ResponseData<List<Shift>>(false, "Có lỗi khi lấy dữ liệu danh sách ca");
			}
		}
		public async Task<ResponseData<string>> CheckIn(int scheduleId, int attendanceID, bool isCheckin, Guid userId)
		{

			try
			{
				string attendanceBy = "";
				if (string.IsNullOrEmpty(attendanceBy))
				{
					return new ResponseData<string>(false, "Lỗi khi xác định người điểm danh");
				}
				var attendance = new EmployeeAttendance();
				if (isCheckin)// kiểm tra diểm danh hay huỷ điểm danh
				{
					if (attendanceID == 0)// nếu điểm danh ms thì tạo ms
					{
						attendance = new EmployeeAttendance()
						{
							CheckInTime = DateTime.Now,
							EmployeeScheduleId = scheduleId,
							OtherNotes = $"Check in bởi: {attendanceBy}"

						};
						await _unitOfWork.EmployeeAttendanceRepository.AddAsync(attendance);
					}
					else// điểm danh trên data có sẵn
					{
						attendance = await _unitOfWork.EmployeeAttendanceRepository.GetAsync(attendanceID);
						attendance.CheckInTime = DateTime.Now;
						await _unitOfWork.EmployeeAttendanceRepository.UpdateAsync(attendance);
					}

				}
				else// nếu là huỷ điểm danh
				{
					attendance = await _unitOfWork.EmployeeAttendanceRepository.GetAsync(attendanceID);
					var timeDitance = DateTime.Now - attendance.CheckInTime;
					if (timeDitance > TimeSpan.FromMinutes(2))//////// kiểm tra thời gian điểm danh cách 2p thì ko thể huỷ
					{
						return new ResponseData<string>(false, "Chỉ có thể huỷ check in trong 2 phút ");
					}
					attendance.CheckInTime = null;
					attendance.OtherNotes = $"Huỷ check in bởi: {attendanceBy}";
					await _unitOfWork.EmployeeAttendanceRepository.UpdateAsync(attendance);

				}
				var result = await _unitOfWork.SaveChangeAsync();
				if (result > 0)
				{
					if (isCheckin)
					{
						return new ResponseData<string>("Check in thành công");
					}
					else
					{
						return new ResponseData<string>("Huỷ Check in thành công");
					}
				}
				return new ResponseData<string>(false, "Thao tác không thành công");
			}

			catch (Exception)
			{
				return new ResponseData<string>(false, "Có lỗi khi kết nối máy chủ");
			}
		}
		public async Task<ResponseData<string>> CheckOut(int attendanceID, bool isCheckout, Guid userId)
		{
			try
			{
				var attendance = new EmployeeAttendance();
				if (isCheckout)// kiểm tra check out hay huỷ check out
				{
					attendance = await _unitOfWork.EmployeeAttendanceRepository.GetAsync(attendanceID);
					if (attendance == null || attendance.CheckInTime == null)
					{
						return new ResponseData<string>(false, "Bạn phải điểm danh trước khi check out");
					}

					attendance.CheckOutTime = DateTime.Now;
					await _unitOfWork.EmployeeAttendanceRepository.UpdateAsync(attendance);


				}
				else// nếu là huỷ điểm danh
				{
					attendance = await _unitOfWork.EmployeeAttendanceRepository.GetAsync(attendanceID);
					var timeDitance = DateTime.Now - attendance.CheckOutTime;
					if (timeDitance > TimeSpan.FromMinutes(2))//////// kiểm tra thời gian checkout cách 2p thì ko thể huỷ
					{
						return new ResponseData<string>(false, "Chỉ có thể huỷ check-out trong 2 phút ");
					}
					attendance.CheckOutTime = null;
					await _unitOfWork.EmployeeAttendanceRepository.UpdateAsync(attendance);

				}
				var result = await _unitOfWork.SaveChangeAsync();
				if (result > 0)
				{
					if (isCheckout)
					{
						return new ResponseData<string>("Check-out thành công");
					}
					else
					{
						return new ResponseData<string>("Huỷ check-out thành công");
					}
				}
				return new ResponseData<string>(false, "Thao tác không thành công");
			}

			catch (Exception)
			{
				return new ResponseData<string>(false, "Có lỗi khi kết nối máy chủ");
			}
		}
		public async Task<ResponseData<List<ShiftVM>>> GetPersonalShift(Guid id)
		{
			try
			{
				var now = DateTime.Now.TimeOfDay;
				var lstShift = await _unitOfWork.ShiftRepository.GetAllAsync();
                var lstWorkShiftToday = await _unitOfWork.WorkShiftRepository.FindAsync(p => p.WorkDate.Date == DateTime.Today.Date);
                var lstSchedule = await _unitOfWork.EmployeeScheduleRepository.GetAllAsync();
                var lstAttendance = await _unitOfWork.EmployeeAttendanceRepository.GetAllAsync();

				var lstCheckin = from s in lstShift
							 join ws in lstWorkShiftToday on s.Id equals ws.ShiftId
							 join es in lstSchedule on ws.Id equals es.WorkShiftId
							 where es.UserId == id
							 select new 
							 {
								 ID = ws.Id,
								 Name =  s.Name,
								 Start = s.From,
								 End = s.To,
								 IsCheckined = ((es.EmployeeAttendances == null || (es.EmployeeAttendances.Count()== 0)) ? false : true),
								 IsCheckouted = ((es.EmployeeAttendances == null || !es.EmployeeAttendances.Any() || es.EmployeeAttendances.First().CheckOutTime==null) ? false : true),
							 };


    //            var lstCheckin2 = (from s in lstShift
    //                             join ws in lstWorkShiftToday on s.Id equals ws.ShiftId
    //                             join es in lstSchedule on ws.Id equals es.WorkShiftId
    //                             where es.UserId == id
    //                             select es).ToList();
				//var demo= (lstCheckin2[1].EmployeeAttendances == null || !lstCheckin2[1].EmployeeAttendances.Any() || lstCheckin2[1].EmployeeAttendances.First().CheckOutTime == null)? false: true;

                var lstShiftQR = new List<ShiftVM>();

				foreach ( var item in lstCheckin)
				{
                    if (!item.IsCheckined && item.End > DateTime.Now.TimeOfDay)
					{
						ShiftVM qr = new ShiftVM() {
							ID = item.ID,
							Name ="Check-in Ca "+ item.Name,
							End = item.End,
							Start = item.Start,
							IsCheckin = true
						};
						lstShiftQR.Add(qr);
					
					}
                    if (!item.IsCheckouted && item.End< DateTime.Now.AddMinutes(10).TimeOfDay)
                    {
                        ShiftVM qr = new ShiftVM()
                        {
                            ID = item.ID,
                            Name = "Check-out Ca " + item.Name,
                            End = item.End,
                            Start = item.Start,
                            IsCheckin = false
                        };
                        lstShiftQR.Add(qr);
                    }
                }
              
				if (lstShiftQR == null || lstShiftQR.Count() == 0)
				{
					return new ResponseData<List<ShiftVM>>(false, "Hiện tại không trong ca làm");
				}
				return new ResponseData<List<ShiftVM>>(lstShiftQR);
			}
			catch (Exception)
			{
				return new ResponseData<List<ShiftVM>>(false, "Bạn đang không có trong danh sách ca làm");
			}
		}
		public async Task<ResponseData<string>> CheckInOutQR(int workShiftID,Guid UserID, bool isCheckIn, string note="")
		{
			try
			{
				var employeSchedule= await _unitOfWork.EmployeeScheduleRepository.FindAsync(p=>p.UserId== UserID&& p.WorkShiftId==workShiftID);
				if (employeSchedule != null&& employeSchedule.Count()>0)// kiểm tra có lịch ko
				{
					int scheduleID = employeSchedule.First().Id;
					var attendances = await _unitOfWork.EmployeeAttendanceRepository.FindAsync(p => p.EmployeeScheduleId == scheduleID);
					var hasAtendance = attendances != null && attendances.Count() > 0;
					if (isCheckIn)// kiểm tra loại điểm danh
					{
						if (hasAtendance)
						{
							return new ResponseData<string>("Bạn đã điểm danh thành công trước đó");
						}
						else
						{
							var attendance = new EmployeeAttendance()
							{
								CheckInTime = DateTime.Now,
								OtherNotes = note,
								EmployeeScheduleId = scheduleID
							};
							await _unitOfWork.EmployeeAttendanceRepository.AddAsync(attendance);
							var result = await _unitOfWork.SaveChangeAsync();
							if (result > 0)
							{
								return new ResponseData<string>("Điểm danh thành công");
							}
						}
					}
					else// checkOut thì
					{
						if (!hasAtendance)
						{
							return new ResponseData<string>("Bạn phải điểm danh trước đó");
						}
						else
						{
							var attendance= attendances!.FirstOrDefault();
							attendance.CheckOutTime= DateTime.Now;
							attendance.OtherNotes= note;
                            await _unitOfWork.EmployeeAttendanceRepository.UpdateAsync(attendance);
							var result = await _unitOfWork.SaveChangeAsync();
							if (result > 0)
							{
								return new ResponseData<string>("Checkout thành công");
							}
						}
					}



                }
				else
				{
					return new ResponseData<string>(false,"Bạn không có lịch hôm nay");
				}
				return new ResponseData<string>(false, "Có lỗi bất ngờ");

			}

			catch (Exception)
			{
				return new ResponseData<string>(false, "Có lỗi khi kết nối máy chủ");
			}
		}
		public async Task<ResponseData<string>> GetTodayCode()
		{
			try
			{
				var today = DateTime.Today.ToString();
				return new ResponseData<string>
				{
					IsSuccess = true,
					Data = _passwordExtensitons.HashCode(today),
				};
			}
			catch (Exception)
			{

				return new ResponseData<string>(false, "Lỗi kết nối đến máy chủ");
			}
		}

        public Task<ResponseData<string>> CheckCode(string code)
        {
			return null; 
        }
    }
}
