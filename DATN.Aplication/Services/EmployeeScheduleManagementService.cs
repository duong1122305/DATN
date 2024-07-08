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
using static System.Runtime.InteropServices.JavaScript.JSType;

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

        public async Task<ResponseData<List<ScheduleView>>> GetUsersInOneMonth(int month, int year)
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
            return new ResponseData<List<ScheduleView>> { IsSuccess = false, Error = $"Chưa có dữ liệu làm việc của tháng {month}/{year} !" };
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
                List<EmployeeSchedule> employeeSchedules = new List<EmployeeSchedule>();
                int count = 0;
                foreach (var user in listUser)
                {
                    foreach (var workShift in query.ToList())
                    {
                        var checkNumberStaffWokingInDay = await CheckNumberOfStaffWokingInDay(shift, workShift.WorkDate);

                        if (checkNumberStaffWokingInDay < 5)
                        {
                            var schedule = new EmployeeSchedule()
                            {
                                UserId = Guid.Parse(user),
                                WorkShiftId = workShift.Id,
                            };
                            var querycheck = from scheduletable in await _unitOfWork.EmployeeScheduleRepository.GetAllAsync()
                                             where scheduletable.UserId == schedule.UserId &&
                                             scheduletable.WorkShiftId == schedule.WorkShiftId
                                             select scheduletable;
                            if (querycheck.ToList().Count == 0)
                            {
                                employeeSchedules.Add(schedule);
                                if (listSuccess.Count == 0)
                                {
                                    listSuccess.Add(schedule.UserId.ToString());
                                }
                                else
                                {
                                    foreach (var item in listSuccess)
                                    {
                                        if (item != schedule.UserId.ToString())
                                        {
                                            count++;
                                        }
                                    }
                                    if (count == listSuccess.Count)
                                    {
                                        listSuccess.Add(schedule.UserId.ToString());
                                        count = 0;
                                    }
                                }
                            }
                        }
                        else
                        {
                            return new ResponseData<string> { IsSuccess = false, Error = "Số người làm việc trong 1 ca vượt quá số người quy định" };
                        }
                    }
                }
                await _unitOfWork.EmployeeScheduleRepository.AddRangeAsync(employeeSchedules);
                return new ResponseData<string> { IsSuccess = true, Data = $"Số người thêm lịch làm việc thành công là: {listSuccess.Count}!" };

            }
            catch (Exception e)
            {
                return new ResponseData<string> { IsSuccess = false, Error = e.Message };
            }
        }
        public async Task<ResponseData<string>> InsertEmployeeCurrentMonth(List<string> listUser, int shift)
        {
            try
            {
                var currentDay = DateTime.Now;
                int nextMonth = currentDay.Month;
                var query = from workshift in await _unitOfWork.WorkShiftRepository.GetAllAsync()
                            where workshift.WorkDate.Month == nextMonth &&
                            workshift.WorkDate.Day >= currentDay.Day &&
                            workshift.ShiftId == shift
                            select workshift;
                List<string> listSuccess = new List<string>();
                List<EmployeeSchedule> employeeSchedules = new List<EmployeeSchedule>();
                int count = 0;
                foreach (var user in listUser)
                {
                    foreach (var workShift in query.ToList())
                    {
                        var checkNumberStaffWokingInDay = await CheckNumberOfStaffWokingInDay(shift, workShift.WorkDate);
                        if (checkNumberStaffWokingInDay < 5)
                        {

                            if (workShift.WorkDate.Day == currentDay.Day && workShift.WorkDate.Month == currentDay.Month && workShift.WorkDate.Year == currentDay.Year)
                            {
                                var queryShift = (from shifttable in await _unitOfWork.ShiftRepository.GetAllAsync()
                                                  where shifttable.Id == shift
                                                  select shifttable).FirstOrDefault();

                                if (currentDay.Hour >= queryShift.To.Hours || currentDay.Hour >= queryShift.From.Hours && currentDay.Hour <= queryShift.To.Hours)
                                {
                                    continue;
                                }
                                else
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
                                        employeeSchedules.Add(schedule);
                                        if (listSuccess.Count == 0)
                                        {
                                            listSuccess.Add(schedule.UserId.ToString());
                                        }
                                        else
                                        {
                                            foreach (var item in listSuccess)
                                            {
                                                if (item != schedule.UserId.ToString())
                                                {
                                                    count++;
                                                }
                                            }
                                            if (count == listSuccess.Count)
                                            {
                                                listSuccess.Add(schedule.UserId.ToString());
                                                count = 0;
                                            }
                                        }
                                    }
                                }
                            }
                            else
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
                                    employeeSchedules.Add(schedule);
                                    if (listSuccess.Count == 0)
                                    {
                                        listSuccess.Add(schedule.UserId.ToString());
                                    }
                                    else
                                    {
                                        foreach (var item in listSuccess)
                                        {
                                            if (item != schedule.UserId.ToString())
                                            {
                                                count++;
                                            }
                                        }
                                        if (count == listSuccess.Count)
                                        {
                                            listSuccess.Add(schedule.UserId.ToString());
                                            count = 0;
                                        }
                                    }
                                }
                            }
                        }
                        else
                        {
                            return new ResponseData<string> { IsSuccess = false, Error = "Số người làm việc trong 1 ca vượt quá số người quy định!!" };
                        }
                    }
                }
                await _unitOfWork.EmployeeScheduleRepository.AddRangeAsync(employeeSchedules);
                return new ResponseData<string> { IsSuccess = true, Data = $"Số người thêm lịch làm việc thành công là: {listSuccess.Count} !" };
            }
            catch (Exception e)
            {
                return new ResponseData<string> { IsSuccess = false, Error = e.Message };
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
                        where user.IsDeleted == false
                        orderby workShift.WorkDate
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
                return new ResponseData<List<ScheduleView>> { IsSuccess = false, Error = "Không có dữ liệu!" };
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
                return new ResponseData<List<ScheduleView>> { IsSuccess = false, Error = "Không có dữ liệu!" };
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
                        where shifttable.Id == shift &&
                        user.IsDeleted == false
                        select new ScheduleView
                        {
                            Name = user.FullName,
                            WorkDate = workShift.WorkDate,
                            Shift = shifttable.Name,
                            To = shifttable.To,
                            From = shifttable.From
                        };
            if (query.Count() > 0) return new ResponseData<List<ScheduleView>> { IsSuccess = true, Data = query.ToList() };
            else return new ResponseData<List<ScheduleView>> { IsSuccess = false, Error = "Chưa có dữ liệu!" };
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
                        && user.IsDeleted == false
                        select new NumberOfScheduleView
                        {
                            IdStaff = user.Id,
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
        public async Task<ResponseData<List<NumberOfScheduleView>>> ListStaffFreeInTime(TimeSpan from1, TimeSpan to)
        {
            if (from1.CompareTo(to) >= 0)
            {
                return new ResponseData<List<NumberOfScheduleView>> { IsSuccess = false, Error = "Giờ bắt đầu lớn hơn giờ kết thúc" };
            }
            else
            {
                var queryShift = (from shift in await _unitOfWork.ShiftRepository.GetAllAsync()
                                  where shift.From.CompareTo(from1) >= 0 && shift.To.CompareTo(to) <= 0
                                  select shift).FirstOrDefault();
                if (queryShift == null)
                {
                    queryShift = (from shift in await _unitOfWork.ShiftRepository.GetAllAsync()
                                 where shift.From > to
                                 select shift).FirstOrDefault();
                }
                var response = await GetListStaffInDay(queryShift.Id, DateTime.Now);
                List<Guid> staffFree = new List<Guid>();
                foreach (var item in response.Data)
                {

                    var queryCheckUser = from bookingDetail in await _unitOfWork.BookingDetailRepository.GetAllAsync()
                                         where bookingDetail.StaffId == item.IdStaff &&
                                         bookingDetail.StartDateTime.TimeOfDay.CompareTo(from1) >= 0 && bookingDetail.EndDateTime.TimeOfDay.CompareTo(to) <= 0
                                         select bookingDetail;
                    if (queryCheckUser.Count() == 0)
                    {
                        staffFree.Add(item.IdStaff.Value);
                    }
                }
                List<NumberOfScheduleView> listStaff = new List<NumberOfScheduleView>();
                foreach (var item in staffFree)
                {
                    var query = await _usermanager.FindByIdAsync(item.ToString());
                    if (query != null)
                    {
                        listStaff.Add(new NumberOfScheduleView { FullName = query.FullName, UserName = query.UserName });
                    }
                }
                if (listStaff.Count > 0)
                    return new ResponseData<List<NumberOfScheduleView>> { IsSuccess = true, Data = listStaff };
                else
                    return new ResponseData<List<NumberOfScheduleView>> { IsSuccess = false, Error = "Giờ này không có nhân viên rảnh để làm r bảo khách phắn" };
            }


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
                        && user.IsDeleted == false
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
                return new ResponseData<List<UserInfView>> { IsSuccess = false, Error = "Không có dữ liệu!" };
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
                await _unitOfWork.EmployeeScheduleRepository.UpdateAsync(user);
                await _unitOfWork.SaveChangeAsync();
                return new ResponseData<string> { IsSuccess = true, Data = "Đổi ca thành công!" };
            }
            else
                return new ResponseData<string> { IsSuccess = false, Error = "Đổi ca thất bại!" };
        }
        private async Task<int> CheckNumberOfStaffWokingInDay(int shift, DateTime workdate)
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
                        select schedule;
            if (query.ToList().Count > 0)
                return query.Count();
            else
                return 0;
        }
    }
}
