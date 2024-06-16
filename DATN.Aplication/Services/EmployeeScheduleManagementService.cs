﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace DATN.Aplication.Services
{
    public class EmployeeScheduleManagementService : IEmployeeScheduleManagementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<User> _usermanager;

        public EmployeeScheduleManagementService(IUnitOfWork unitOfWork, UserManager<User> userManager)
        {
            _unitOfWork = unitOfWork;
            _usermanager = userManager;
        }

        public async Task<ResponseData<List<ScheduleView>>> GetUserInOneMonth(int month, int year)
        {
            var query = from shift in await _unitOfWork.ShiftRepository.GetAllAsync()
                        join workshift in await _unitOfWork.WorkShiftRepository.GetAllAsync()
                        on shift.Id equals workshift.ShiftId
                        join schedule in await _unitOfWork.EmployeeScheduleRepository.GetAllAsync()
                        on workshift.Id equals schedule.WorkShiftId
                        join user in await _usermanager.Users.ToListAsync()
                        on schedule.UserId equals user.Id
                        where workshift.WorkDate.Year == year &&
                        workshift.WorkDate.Month == month
                        select new ScheduleView
                        {
                            Name = user.FullName,
                            WorkDate = workshift.WorkDate,
                            Shift = shift.Name,
                            From = shift.From,
                            To = shift.To
                        };
            if (query.Count() > 0)
            {
                return new ResponseData<List<ScheduleView>> { IsSuccess = true, Data = query.ToList() };
            }
            return new ResponseData<List<ScheduleView>> { IsSuccess = true, Error = $"Chưa có dữ liệu làm việc của tháng {month}/{year}" };
        }

        public async Task<ResponseData<string>> InsertEmployeeNextMonthCompareCurrentMonth(List<string> listUser, int shift)
        {
            try
            {
                var currentDay = DateTime.Now;
                int nextMonth = currentDay.Month == 12 ? 1 : currentDay.Month + 1;
                int nextYear = nextMonth == 12 ? currentDay.Year + 1 : currentDay.Year;

                var query = from workshift in await _unitOfWork.WorkShiftRepository.GetAllAsync()
                            where workshift.WorkDate.Year == nextYear &&
                            workshift.WorkDate.Month == nextMonth &&
                            workshift.ShiftId == shift
                            select workshift;
                List<string> listSuccess = new List<string>();
                int count = 0;
                foreach (var user in listUser)
                {
                    foreach (var workShift in query)
                    {
                        var schedule = new EmployeeSchedule()
                        {
                            UserId = Guid.Parse(user),
                            WorkShiftId = workShift.Id
                        };
                        var querycheck = from scheduletable in await _unitOfWork.EmployeeScheduleRepository.GetAllAsync()
                                         where scheduletable.UserId == schedule.UserId &&
                                         scheduletable.WorkShiftId == schedule.WorkShiftId
                                         select scheduletable;
                        if (querycheck.ToList().Count == 0)
                        {
                            await _unitOfWork.EmployeeScheduleRepository.AddAsync(schedule);
                            await _unitOfWork.EmployeeScheduleRepository.SaveChangesAsync();
                            foreach (var item in listSuccess)
                            {
                                if (item != schedule.UserId.ToString())
                                {
                                    listSuccess.Add(schedule.UserId.ToString());
                                }
                            }
                        }
                    }
                }
                return new ResponseData<string> { IsSuccess = true, Data = $"Số người thêm lịch làm việc thành công là: {listSuccess.Count}!" };
            }
            catch (Exception e)
            {
                return new ResponseData<string> { IsSuccess = true, Error = e.Message };
            }
        }

        public async Task<ResponseData<List<ScheduleView>>> GetAll()
        {
            var query = from shifttable in await _unitOfWork.ShiftRepository.GetAllAsync()
                        join workShift in await _unitOfWork.WorkShiftRepository.GetAllAsync()
                        on shifttable.Id equals workShift.ShiftId
                        join schedule in await _unitOfWork.EmployeeScheduleRepository.GetAllAsync()
                        on workShift.Id equals schedule.WorkShiftId
                        join user in await _usermanager.Users.ToListAsync()
                        on schedule.UserId equals user.Id
                        group new { schedule.WorkShiftId, shifttable.Name, workShift.WorkDate }
                        by new { shifttable.Name, workShift.WorkDate, shifttable.From, shifttable.To, shifttable.Id }
                        into view
                        select new ScheduleView
                        {
                            ShiftID = view.Key.Id,
                            Name = view.Count().ToString() + " người",
                            WorkDate = view.Key.WorkDate,
                            Shift = view.Key.Name,
                            To = view.Key.To,
                            From = view.Key.From,
                        };

            if (query.Count() > 0)
                return new ResponseData<List<ScheduleView>> { IsSuccess = true, Data = query.ToList() };
            else
                return new ResponseData<List<ScheduleView>> { IsSuccess = false, Error = "Kh có dữ liệu" };
        }

        public async Task<ResponseData<List<ScheduleView>>> GetScheduleFromMonthToMonth(ScheduleMonthToMonthView scheduleMonthToMonthView)
        {
            var query = from shifttable in await _unitOfWork.ShiftRepository.GetAllAsync()
                        join workShift in await _unitOfWork.WorkShiftRepository.GetAllAsync()
                        on shifttable.Id equals workShift.ShiftId
                        join schedule in await _unitOfWork.EmployeeScheduleRepository.GetAllAsync()
                        on workShift.Id equals schedule.WorkShiftId
                        join user in await _usermanager.Users.ToListAsync()
                        on schedule.UserId equals user.Id
                        where workShift.WorkDate.Year >= scheduleMonthToMonthView.YearFrom &&
                        workShift.WorkDate.Month >= scheduleMonthToMonthView.MonthFrom &&
                        workShift.WorkDate.Year <= scheduleMonthToMonthView.YearTo &&
                        workShift.WorkDate.Month <= scheduleMonthToMonthView.MonthTo
                        select new ScheduleView
                        {
                            Name = user.FullName,
                            WorkDate = workShift.WorkDate,
                            Shift = shifttable.Name,
                            To = shifttable.To,
                            From = shifttable.From
                        };
            if (query.Count() > 0)
                return new ResponseData<List<ScheduleView>> { IsSuccess = true, Data = query.ToList() };
            else
                return new ResponseData<List<ScheduleView>> { IsSuccess = false, Error = "Kh có dữ liệu" };
        }

        public async Task<ResponseData<List<ScheduleView>>> GetScheduleForShift(int shift)
        {
            var query = from shifttable in await _unitOfWork.ShiftRepository.GetAllAsync()
                        join workShift in await _unitOfWork.WorkShiftRepository.GetAllAsync()
                        on shifttable.Id equals workShift.ShiftId
                        join schedule in await _unitOfWork.EmployeeScheduleRepository.GetAllAsync()
                        on workShift.Id equals schedule.WorkShiftId
                        join user in await _usermanager.Users.ToListAsync()
                        on schedule.UserId equals user.Id
                        where shifttable.Id == shift
                        select new ScheduleView
                        {
                            Name = user.FullName,
                            WorkDate = workShift.WorkDate,
                            Shift = shifttable.Name,
                            To = shifttable.To,
                            From = shifttable.From
                        };
            if (query.Count() > 0) return new ResponseData<List<ScheduleView>> { IsSuccess = true, Data = query.ToList() };
            else return new ResponseData<List<ScheduleView>> { IsSuccess = false, Error = "Chưa có dữ liệu" };
        }
        public async Task<ResponseData<List<NumberOfScheduleView>>> GetListStaffInDay(int shift, DateTime workdate)
        {
            var query = from shifttable in await _unitOfWork.ShiftRepository.GetAllAsync()
                        join workShift in await _unitOfWork.WorkShiftRepository.GetAllAsync()
                        on shifttable.Id equals workShift.ShiftId
                        join schedule in await _unitOfWork.EmployeeScheduleRepository.GetAllAsync()
                        on workShift.Id equals schedule.WorkShiftId
                        join user in await _usermanager.Users.ToListAsync()
                        on schedule.UserId equals user.Id
                        where shifttable.Id == shift && workShift.WorkDate.Year == workdate.Year
                        && workShift.WorkDate.Month == workdate.Month && workShift.WorkDate.Day == workdate.Day
                        select new NumberOfScheduleView
                        {
                            FullName = user.FullName,
                            UserName = user.UserName,
                            Date = workShift.WorkDate,
                            ShiftName = shifttable.Name,
                            shiftId = shifttable.Id,
                        };
            if (query.ToList().Count > 0)
                return new ResponseData<List<NumberOfScheduleView>> { IsSuccess = true, Data = query.ToList() };
            else
                return new ResponseData<List<NumberOfScheduleView>> { IsSuccess = false, Error = $"Không có dữ liệu về nhân viên trong ca" };
        }
        public async Task<ResponseData<List<UserInfView>>> ListStaffNotWorkingInDay(int shiftId, DateTime workDate)
        {
            var query = from shifttable in await _unitOfWork.ShiftRepository.GetAllAsync()
                        join workShift in await _unitOfWork.WorkShiftRepository.GetAllAsync()
                        on shifttable.Id equals workShift.ShiftId
                        join schedule in await _unitOfWork.EmployeeScheduleRepository.GetAllAsync()
                        on workShift.Id equals schedule.WorkShiftId
                        join user in await _usermanager.Users.ToListAsync()
                        on schedule.UserId equals user.Id
                        where shifttable.Id == shiftId &&
                        workShift.WorkDate.Year == workDate.Year &&
                        workShift.WorkDate.Month == workDate.Month &&
                        workShift.WorkDate.Day == workDate.Day
                        select new UserInfView
                        {
                            UserName = user.UserName,
                            FullName = user.FullName,
                        };
            var listUser = new List<UserInfView>();
            foreach (var item in await _usermanager.Users.ToListAsync())
            {
                int count = 0;
                foreach (var item2 in query.ToList())
                {
                    if (item.UserName != item2.UserName)
                    {
                        count++;
                    }
                }
                if (count == query.ToList().Count)
                {
                    listUser.Add(new UserInfView
                    {
                        UserName = item.UserName,
                        FullName = item.FullName,
                    });
                }
            }
            if (listUser.Count > 0)
                return new ResponseData<List<UserInfView>> { Data = listUser, IsSuccess = true };
            else
                return new ResponseData<List<UserInfView>> { IsSuccess = false, Error = "Không có dữ liệu" };
        }

        public async Task<ResponseData<string>> ChangeShiftStaffToStaff(ChangeShiftView changeShiftView)
        {
            var query = from shifttable in await _unitOfWork.ShiftRepository.GetAllAsync()
                        join workShift in await _unitOfWork.WorkShiftRepository.GetAllAsync()
                        on shifttable.Id equals workShift.ShiftId
                        join schedule in await _unitOfWork.EmployeeScheduleRepository.GetAllAsync()
                        on workShift.Id equals schedule.WorkShiftId
                        join user in await _usermanager.Users.ToListAsync()
                        on schedule.UserId equals user.Id
                        where user.Id == Guid.Parse(changeShiftView.UserIdFirst) &&
                        shifttable.Id == changeShiftView.ShiftId &&
                        workShift.WorkDate.Year == changeShiftView.Date.Year && workShift.WorkDate.Month == changeShiftView.Date.Month
                        && workShift.WorkDate.Day == changeShiftView.Date.Day
                        select schedule;
            if (query.ToList().Count == 1)
            {

                var user = query.FirstOrDefault();
                user.UserId = Guid.Parse(changeShiftView.UserIdSecond);
                _unitOfWork.EmployeeScheduleRepository.UpdateAsync(user);
                _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Đổi ca thành công" };
            }
            else
                return new ResponseData<string> { IsSuccess = false, Error = "Đổi ca thất bại" };
        }
    }
}
