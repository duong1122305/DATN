using System;
using System.Collections.Generic;
using System.Linq;
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

        public async Task<ResponseData<List<ScheduleView>>> GetAll(int month, int year)
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
                            workshift.WorkDate.Month == nextMonth
                            select workshift;

                foreach (var workShift in query)
                {
                    foreach (var user in listUser)
                    {
                        var schedule = new EmployeeSchedule()
                        {
                            UserId = Guid.Parse(user),
                            WorkShiftId = workShift.Id
                        };
                        await _unitOfWork.EmployeeScheduleRepository.AddAsync(schedule);
                        await _unitOfWork.EmployeeScheduleRepository.SaveChangesAsync();
                    }
                }
                return new ResponseData<string> { IsSuccess = true, Data = $"Thêm lịch làm việc ca" };
            }
            catch (Exception e)
            {
                return new ResponseData<string> { IsSuccess = true, Error = e.Message };
            }
        }
    }
}
