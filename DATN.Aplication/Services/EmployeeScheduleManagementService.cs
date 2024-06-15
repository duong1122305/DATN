using System;
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
                        by new { shifttable.Name, workShift.WorkDate, shifttable.From, shifttable.To }
                        into view
                        select new ScheduleView
                        {
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
    }
}
