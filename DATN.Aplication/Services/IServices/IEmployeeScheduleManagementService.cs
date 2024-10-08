﻿using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.Aplication.Services.IServices
{
    public interface IEmployeeScheduleManagementService
    {
        public Task<ResponseData<string>> InsertEmployeeNextMonthCompareCurrentMonth(List<string> listUser, int shift);
        public Task<ResponseData<string>> InsertEmployeeCurrentMonth(List<string> listUser, int shift);
        public Task<ResponseData<List<ScheduleView>>> GetUsersInOneMonth(int month, int year);
        public Task<ResponseData<List<ScheduleView>>> GetScheduleForShift(int shift);
        public Task<ResponseData<List<ScheduleView>>> GetAll();
        public Task<ResponseData<List<ScheduleView>>> GetScheduleFromMonthToMonth(ScheduleMonthToMonthView scheduleMonthToMonthView);
        public Task<ResponseData<List<NumberOfScheduleView>>> GetListStaffInDay(int shift, DateTime workdate);
        public Task<ResponseData<List<UserInfView>>> ListStaffNotWorkingInDay(int shiftId, DateTime workDate, string role);
        public Task<ResponseData<string>> ChangeShiftStaffToStaff(ChangeShiftView changeShiftView);
        public Task<ResponseData<List<NumberOfScheduleView>>> ListStaffFreeInTime(TimeSpan from1, TimeSpan to, DateTime dateTime);
        public Task<ResponseData<string>> InsertOneDayScheduleForStaffSuddenly(List<string> listUser, int shift, DateTime dateTime);


    }
}