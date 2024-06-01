using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Azure;
using DATN.Aplication.Services.IServices;
using DATN.Data.Entities;
using DATN.ViewModels.Common;

namespace DATN.Aplication.Services
{
    public class WorkShiftManagementService : IWorkShiftManagementService
    {
        private readonly IUnitOfWork _unitOfWork;

        public WorkShiftManagementService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ResponseData<string>> InsertWorkShiftNextMonthCompareCurrentMonth()
        {
            var currentDay = DateTime.Now;
            int nextMonth = currentDay.Month == 12 ? 1 : currentDay.Month + 1;
            int nextYear = nextMonth == 12 ? currentDay.Year + 1 : currentDay.Year;
            var query = from workshift in await _unitOfWork.WorkShiftRepository.GetAllAsync()
                        where workshift.WorkDate.Year == nextYear &&
                        workshift.WorkDate.Month == nextMonth
                        select workshift;
            if (query == null)
            {
                try
                {
                    if (currentDay.Month == 12)
                    {
                        var dayInMonth = DateTime.DaysInMonth(currentDay.Year + 1, 1);
                        for (var i = 1; i <= dayInMonth; i++)
                        {
                            foreach (var day in await _unitOfWork.ShiftRepository.GetAllAsync())
                            {
                                var workshift = new WorkShift()
                                {
                                    ShiftId = day.Id,
                                    WorkDate = new DateTime(currentDay.Year + 1, 1, i)
                                };
                                await _unitOfWork.WorkShiftRepository.AddAsync(workshift);
                                await _unitOfWork.WorkShiftRepository.SaveChangesAsync();
                            }
                        }
                    }
                    else
                    {
                        var dayInMonth = DateTime.DaysInMonth(currentDay.Year, currentDay.Month + 1);
                        for (var i = 1; i <= dayInMonth; i++)
                        {
                            foreach (var day in await _unitOfWork.ShiftRepository.GetAllAsync())
                            {
                                var workshift = new WorkShift()
                                {
                                    ShiftId = day.Id,
                                    WorkDate = new DateTime(currentDay.Year, currentDay.Month + 1, i)
                                };
                                await _unitOfWork.WorkShiftRepository.AddAsync(workshift);
                                await _unitOfWork.WorkShiftRepository.SaveChangesAsync();
                            }
                        }
                    }
                    return new ResponseData<string> { IsSuccess = true, Data = "Thêm xong lịch 30 ngày" };
                }
                catch (Exception e)
                {
                    return new ResponseData<string> { IsSuccess = false, Data = e.Message };
                }
            }
            else
            {
                return new ResponseData<string> { IsSuccess = false, Error = $"Tháng {nextMonth} của {nextYear} đã có rồi" };
            }
        }
    }
}
