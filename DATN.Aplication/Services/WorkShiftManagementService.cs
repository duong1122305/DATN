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
            if (query.ToList().Count == 0)
            {
                try
                {
                    List<WorkShift> listSuccess = new List<WorkShift>();
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
                                listSuccess.Add(workshift);
                            }
                        }
                        await _unitOfWork.WorkShiftRepository.AddRangeAsync(listSuccess);
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
                                listSuccess.Add(workshift);
                            }
                        }
                        await _unitOfWork.WorkShiftRepository.AddRangeAsync(listSuccess);
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
        public async Task<ResponseData<string>> InsertWorkShiftCurrentMonth()
        {
            var currentDay = DateTime.Now;
            int currentMonth = currentDay.Month;
            var query = from workshift in await _unitOfWork.WorkShiftRepository.GetAllAsync()
                        where workshift.WorkDate.Month == currentMonth
                        select workshift;
            if (query.ToList().Count == 0)
            {
                try
                {
                    List<WorkShift> listSuccess = new List<WorkShift>();
                    var dayInMonth = DateTime.DaysInMonth(currentDay.Year, currentDay.Month);
                    for (var i = 1; i <= dayInMonth; i++)
                    {
                        foreach (var day in await _unitOfWork.ShiftRepository.GetAllAsync())
                        {
                            var workshift = new WorkShift()
                            {
                                ShiftId = day.Id,
                                WorkDate = new DateTime(currentDay.Year, currentDay.Month, i)
                            };
                            listSuccess.Add(workshift);
                        }
                    }
                    await _unitOfWork.WorkShiftRepository.AddRangeAsync(listSuccess);
                    return new ResponseData<string> { IsSuccess = true, Data = "Thêm xong lịch 30 ngày" };
                }
                catch (Exception e)
                {
                    return new ResponseData<string> { IsSuccess = false, Data = e.Message };
                }
            }
            else
            {
                return new ResponseData<string> { IsSuccess = false, Error = $"Tháng {currentMonth} của {currentDay.Year} đã có rồi" };
            }
        }
    }
}
