﻿using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Attendace;
using DATN.ViewModels.Enum;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DATN.Aplication.Services
{
    public class AttendanteMangarService: IAttendanteMangarService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _userManager;

        public AttendanteMangarService(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }
        public async Task<ResponseData<List<AttendanceViewModel>>> ListAttendanceToday(int shiftID)
        {
            try
            {
                var lstWorkShiftToday = await _unitOfWork.WorkShiftRepository.FindAsync(p=>p.WorkDate.Date==DateTime.Today.Date);
                var lstSchedule = await  _unitOfWork.EmployeeScheduleRepository.GetAllAsync();
                var lstAttendance = await  _unitOfWork.EmployeeAttendanceRepository.GetAllAsync();
                var lstUser = _userManager.Users.ToList();
                var result = from user in lstUser
                             join st in lstSchedule on user.Id equals st.UserId
                             join ws in lstWorkShiftToday on st.WorkShiftId equals ws.Id
                             join at in lstAttendance on ws.Id equals at.EmployeeScheduleId into attendanceData
                             from at in attendanceData.DefaultIfEmpty()
                             where ws.ShiftId == shiftID || shiftID == 0
                             select new AttendanceViewModel
                             {
                                 UserName = user.UserName,
                                 DateAttendace = DateTime.Today.ToString("dd/MM/yyyy"),
                                 CheckInTime = at != null  && at.CheckInTime.HasValue ? at.CheckInTime.Value.ToString("hh:mm") : "0",
                                 CheckOutTime = at != null && at.CheckOutTime.HasValue ? at.CheckOutTime.Value.ToString("hh:mm") : "0",
                                 ID = at != null ? at.Id : 0,
                                 StaffName = user.FullName,
                                 ScheduleID = st.Id,
                             };
                var data = result.ToList();
                if (result== null|| result.Count()==0)
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
                var lstShift= await _unitOfWork.ShiftRepository.GetAllAsync();
                lstShift = lstShift.Where(p => p.From.Add(TimeSpan.FromMinutes(-15)) < now && p.To>now);
                if (lstShift == null || lstShift.Count()==0)
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
        public async Task<ResponseData<string>> CheckIn(int scheduleId, int attendanceID, bool isCheckin)
        {
            if (isCheckin)
            {
                var attendance = new EmployeeAttendance();
                if (attendanceID==0)
                {
                    attendance = new EmployeeAttendance() 
                    { 
                        CheckInTime = DateTime.Now,
                        EmployeeScheduleId = scheduleId,
                        
                    };
                    await _unitOfWork.EmployeeAttendanceRepository.AddAsync(attendance);
                }
                else
                {
                    attendance =await _unitOfWork.EmployeeAttendanceRepository.GetAsync(attendanceID);
                }

            }

            return null;
        }
    }
}