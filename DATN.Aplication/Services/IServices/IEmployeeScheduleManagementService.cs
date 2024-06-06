﻿using DATN.ViewModels.Common;
using DATN.ViewModels.DTOs.Authenticate;

namespace DATN.Aplication.Services.IServices
{
    public interface IEmployeeScheduleManagementService
    {
        public Task<ResponseData<string>> InsertEmployeeNextMonthCompareCurrentMonth(List<string> listUser, int shift);
        public Task<ResponseData<List<ScheduleView>>> GetUserInOneMonth(int month, int year);
        public Task<ResponseData<List<ScheduleView>>> GetScheduleForShift(int shift);
        public Task<ResponseData<List<ScheduleView>>> GetAll();
        public Task<ResponseData<List<ScheduleView>>> GetScheduleFromMonthToMonth(ScheduleMonthToMonthView scheduleMonthToMonthView);

    }
}